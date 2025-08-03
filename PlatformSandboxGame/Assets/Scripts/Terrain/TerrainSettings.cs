using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariaClone.Terrain
{
    [CreateAssetMenu(fileName = "TerrainSettings", menuName = "Resources/TerrainSettings")]
    public class TerrainSettings : ScriptableObject
    {
        [field: Tooltip("The seed for the world, leave as 0 for random seed")]
        [field: SerializeField] public float seed { get; private set; }
        [field: Tooltip("Mountain height multiplier (the higher this is, steeper the mountains)")]
        [field: SerializeField] public float heightMultiplier { get; private set; }
        [field: Tooltip("Addition to the overall height of the world, just makes the world taller")]
        [field: SerializeField] public int heightAddition { get; private set; }
        [field: Tooltip("Number of dirt tiles between grass tile on the terrain surface, and the stone layer")]
        [field: SerializeField] public int dirtLayerHeight { get; private set; }
        [field: Tooltip("The frequency of the perlin noise for generating terrain mountains")]
        [field: Range(0,1)] [field: SerializeField] public float terrainFrequency { get; private set; }
        [field: Tooltip("The frequency of the perlin noise for generating terrain caves")]
        [field: Range(0, 1)] [field: SerializeField] public float caveFrequency { get; private set; }
        [field: Tooltip("X and Y size of the world, bigger this is, the longer the terrain will take to generate")]
        [field: SerializeField] public Vector2Int worldSize { get; private set; }
        [field: Tooltip("Percent chance for tall grass to spawn")]
        [field: Range(0, 100)] [field: SerializeField] public int tallGrassChance { get; private set; }
        [field: Tooltip("Percent chance for trees to spawn")]
        [field: Range(0, 100)] [field: SerializeField] public int treeChance { get; private set; }
        [field: Tooltip("Percent chance for vines to spawn")]
        [field: Range(0, 100)] [field: SerializeField] public int vineChance { get; private set; }
        [field: Tooltip("Percent chance for stalactites (in caves) to spawn")]
        [field: Range(0, 100)] [field: SerializeField] public int stalactiteChance { get; private set; }
        [field: Tooltip("Percent chance for pots or treasure to spawn")]
        [field: Range(0, 100)] [field: SerializeField] public int potChance { get; private set; }
        [field: Tooltip("Define all the ores for the world here")]
        [field: SerializeField] public OreClass[] ores { get; private set; }
        //hidden in inspector
        public bool[,] cavePoints { get; private set; }

        public void Init()
        {
            cavePoints = new bool[worldSize.x, worldSize.y];

            RecalculateSeed();
            InitCaves();
            InitOres();
        }

        private void RecalculateSeed() { if (seed == 0) seed = Random.Range(-10000, 10000); }
        private void InitCaves()
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                float h = GetHeight(x);
                for (int y = 0; y < worldSize.y; y++)
                {
                    float p = y / h;
                    float v = Mathf.PerlinNoise((x + seed) * caveFrequency, (y + seed) * caveFrequency);
                    v *= p + 0.5f;
                    cavePoints[x, y] = v > 0.5f && y <= GetHeight(x);
                }
            }
        }
        private void InitOres()
        {
            foreach (OreClass ore in ores)
                ore.Init(worldSize, seed);
        }
        public int GetHeight(float x)
        {
            return heightAddition + 
                Mathf.CeilToInt(heightMultiplier * 
                Mathf.PerlinNoise((x + seed) * terrainFrequency, seed * terrainFrequency));
        }
    }
}