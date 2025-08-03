using TerrariaClone.Items;
using UnityEngine;

namespace TerrariaClone.Terrain
{
    [System.Serializable]
    public class OreClass
    {
        [field: Tooltip("The tile to place for this ore")]
        [field: SerializeField] public TileClass oreTile { get; private set; }
        [field: Tooltip("Highest Y position the tile can be found. Make sure to keep this below the world Y size")]
        [field: SerializeField] public int maxSpawnHeight { get; private set; }
        [field: Tooltip("Lowest Y position the tile can be found. Where 0 is the bottom of the world")]
        [field: SerializeField] public int minSpawnHeight { get; private set; }
        [field: Tooltip("Perlin noise frequency of this ore")]
        [field: SerializeField] [field: Range(0,1)] public float spawnFrequency { get; private set; }
        [field: Tooltip("Threshold of the perlin noise value for this ore to spawn. Make this higher to get larger clusters of ore")]
        [field: SerializeField] [field: Range(0, 1)] public float spawnRadius { get; private set; }
        public bool[,] spawnMask { get; private set; }

        public void Init(Vector2Int worldSize, float seed)
        {
            spawnMask = new bool[worldSize.x, worldSize.y];
            DrawSpawnMask(worldSize, seed);
        }

        private void DrawSpawnMask(Vector2Int worldSize, float seed)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                for (int y = 0; y < worldSize.y; y++)
                {
                    float v = Mathf.PerlinNoise((x + seed + oreTile.name[0]) * spawnFrequency,
                        (y + seed + oreTile.name[0]) * spawnFrequency);
                    spawnMask[x, y] = v <= spawnRadius && y <= maxSpawnHeight && y >= minSpawnHeight;
                }
            }
        }
    }
}
