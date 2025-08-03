using System.Collections;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;

namespace TerrariaClone.Terrain
{
    public class LightSolver : MonoBehaviour
    {
        [Tooltip("between 0 & 15")] [SerializeField] private float sunlightBrightness = 15f;
        [SerializeField] private int iterations = 7;
        [SerializeField] private Transform lightMapOverlay;
        private TerrainGenerator terrainGenerator;
        private TerrainSettings terrainSettings;
        private float[,] lightValues;
        private Texture2D lightMap;
        public Material lightShader;

        public bool ReCalculate;

        private void OnValidate() //update lighting if we press the recalculate check box
        {
            if (ReCalculate)
            {
                IUpdate();
                ReCalculate = false;
            }
        }

        public void Init() //set values
        {
            terrainGenerator = GetComponent<TerrainGenerator>();
            terrainSettings = terrainGenerator.terrainSettings;
            lightValues = new float[terrainSettings.worldSize.x, terrainSettings.worldSize.y];
            lightMap = new Texture2D(terrainSettings.worldSize.x, terrainSettings.worldSize.y);
            lightMapOverlay.localScale = new Vector3(terrainSettings.worldSize.x, terrainSettings.worldSize.y, 1);
            lightMapOverlay.position = new Vector2(terrainSettings.worldSize.x / 2f, terrainSettings.worldSize.y / 2f);
            lightShader.SetTexture("_LightMap", lightMap);

            lightMap.filterMode = FilterMode.Point; //< remove this line for smooth lighting, keep it for tiled lighting
        }

        public void IUpdate() //call this method for any lighting updates
        {
            StopCoroutine(UpdateLighting());
            StartCoroutine(UpdateLighting());
        }

        private IEnumerator UpdateLighting() //calculate the new light values for every tile in the world
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < iterations; i++)
            {
                for (int x = 0; x < terrainSettings.worldSize.x; x++)
                {
                    float lightLevel = sunlightBrightness;
                    for (int y = terrainSettings.worldSize.y - 1; y >= 0; y--)
                    {
                        if (terrainGenerator.isIlluminate(x, y) ||
                            (terrainGenerator.tileData[x, y, 1] is null &&
                                terrainGenerator.tileData[x, y, 2] is null)) //if illuminate block
                            lightLevel = sunlightBrightness;
                        else
                        {
                            //find brightest neighbour
                            int nx1 = Mathf.Clamp(x - 1, 0, terrainSettings.worldSize.x - 1);
                            int nx2 = Mathf.Clamp(x + 1, 0, terrainSettings.worldSize.x - 1);
                            int ny1 = Mathf.Clamp(y - 1, 0, terrainSettings.worldSize.y - 1);
                            int ny2 = Mathf.Clamp(y + 1, 0, terrainSettings.worldSize.y - 1);

                            lightLevel = Mathf.Max(
                                lightValues[nx1, y],
                                lightValues[nx2, y],
                                lightValues[x, ny1],
                                lightValues[x, ny2]);

                            if (terrainGenerator.tileData[x, y, 1] is null)
                                lightLevel -= 0.75f;
                            else
                                lightLevel -= 2.5f;
                        }

                        lightValues[x, y] = lightLevel;
                    }
                }

                //reverse calculation to remove artifacts
                for (int x = terrainSettings.worldSize.x - 1; x > 0; x--)
                {
                    float lightLevel;
                    for (int y = 0; y < terrainSettings.worldSize.y; y++)
                    {
                        //find brightest neighbour
                        int nx1 = Mathf.Clamp(x - 1, 0, terrainSettings.worldSize.x - 1);
                        int nx2 = Mathf.Clamp(x + 1, 0, terrainSettings.worldSize.x - 1);
                        int ny1 = Mathf.Clamp(y - 1, 0, terrainSettings.worldSize.y - 1);
                        int ny2 = Mathf.Clamp(y + 1, 0, terrainSettings.worldSize.y - 1);

                        lightLevel = Mathf.Max(
                            lightValues[nx1, y],
                            lightValues[nx2, y],
                            lightValues[x, ny1],
                            lightValues[x, ny2]);

                        if (terrainGenerator.tileData[x, y, 1] is null)
                            lightLevel -= 0.75f;
                        else
                            lightLevel -= 2.5f;

                        lightValues[x, y] = lightLevel;
                    }
                }
            }

            //apply to texture
            for (int x = 0; x < terrainSettings.worldSize.x; x++)
            {
                for (int y = 0; y < terrainSettings.worldSize.y; y++)
                {
                    lightMap.SetPixel(x, y, new Color(0, 0, 0, 1 - (lightValues[x, y] / 15f)));
                }
            }
            lightMap.Apply(); //send new texture to the GPU so that we can render it on screen
        }
    }
}
