using JetBrains.Annotations;
using System.Collections;
using TerrariaClone.Enums;
using TerrariaClone.Items;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TerrariaClone.Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Tooltip("Tilemap layers which our world generates in")]
        [SerializeField] private Tilemap[] tileMaps;   
            
        //hidden in inspector
        [HideInInspector] public TerrainSettings terrainSettings; //obtained via Resources
        [HideInInspector] public TileClass[,,] tileData; //obtained via Resources
        public TileAtlas tileAtlas { get; private set; }
        private LightSolver lighting;

        private void Awake() => Init();

        public void Init()
        {
            terrainSettings = Resources.Load<TerrainSettings>("TerrainSettings");
            tileAtlas = Resources.Load<TileAtlas>("TileAtlas");
            tileData = new TileClass[terrainSettings.worldSize.x, terrainSettings.worldSize.y, tileMaps.Length];
            lighting = GetComponent<LightSolver>();

            terrainSettings.Init();
            lighting.Init();

            Generate();

            lighting.IUpdate();
        }

        private void Generate() {
            // Vòng lặp 1: Generation địa hình cơ bản (đất, đá, cỏ)
            GenerateTerrain();

            // Vòng lặp 2: Generation addon (cây, dây leo, vật phẩm)
            GenerateAddons();
        }

        private void GenerateTerrain() {
            for (int x = 0; x < terrainSettings.worldSize.x; x++) {
                float height = terrainSettings.GetHeight(x);
                for (int y = 0; y < height; y++) {
                    TileClass tileToPlace;

                    if (y < height - terrainSettings.dirtLayerHeight)
                        tileToPlace = tileAtlas.stone;
                    else if (y < height - 3)
                        tileToPlace = tileAtlas.dirt;
                    else
                        tileToPlace = tileAtlas.grass;

                    // Quặng
                    foreach (OreClass ore in terrainSettings.ores) {
                        if (ore.spawnMask[x, y])
                            tileToPlace = ore.oreTile;
                    }

                    // Đặt tile nếu không phải hang động
                    if (terrainSettings.cavePoints[x, y])
                        PlaceTile(tileToPlace, x, y);

                    // Đặt tường
                    if (y < height - terrainSettings.dirtLayerHeight - Random.Range(2, 5))
                        PlaceTile(tileAtlas.stoneWall, x, y);
                    else if (y < height - Random.Range(2, 3))
                        PlaceTile(tileAtlas.dirtWall, x, y);
                }
            }
        }

        private void GenerateAddons() {
            for (int x = 0; x < terrainSettings.worldSize.x; x++) {
                float height = terrainSettings.GetHeight(x);
                int surfaceY = Mathf.FloorToInt(height) - 1;

                // Xử lý addon trên bề mặt
                if (tileData[x, surfaceY, 1] == tileAtlas.grass) {
                    // Cỏ cao
                    if (Random.Range(0, 100) < terrainSettings.tallGrassChance)
                        PlaceTile(tileAtlas.tallGrass, x, surfaceY + 1);
                    // Cây
                    else if (Random.Range(0, 100) < terrainSettings.treeChance)
                        SpawnTree(x, surfaceY + 1);

                    // Dây leo
                    SpawnVinesFromSurface(x, surfaceY);
                }

                // Xử lý addon dưới lòng đất
                for (int y = 0; y < surfaceY; y++) {
                    // Thạch nhũ
                    if (tileData[x, y, 1] == tileAtlas.stone &&
                        tileData[x, y - 1, 1] is null &&
                        Random.Range(0, 100) <= terrainSettings.stalactiteChance) {
                        PlaceTile(tileAtlas.stalactite, x, y);
                    }

                    // Chậu
                    if (x < terrainSettings.worldSize.x - 2 &&
                        terrainSettings.cavePoints[x, y] &&
                        terrainSettings.cavePoints[x + 1, y] &&
                        !terrainSettings.cavePoints[x, y + 1] &&
                        Random.Range(0, 100) <= terrainSettings.potChance) {
                        PlaceTile(tileAtlas.pot, x, y + 1);
                    }
                }
            }
        }

        private void SpawnVinesFromSurface(int x, int surfaceY) {
            int i = 0;
            while (i < 4) {
                if (tileData[x, surfaceY - i, 1] is null) {
                    SpawnVine(x, surfaceY - i);
                    break;
                }
                i++;
            }
        }

        private void SpawnTree(int x, int y) //tree schematic
        {
            // Kiểm tra biên
            if (x < 2 || x >= terrainSettings.worldSize.x - 2) return;

            int height = Random.Range(8, 15);

            // Kiểm tra khoảng cách với các cây khác (2 tile mỗi phía)
            for (int i = 0; i < height; i++) {
                for (int j = -2; j <= 0; j++) {
                    if (tileData[x + j, y + i, 0] == tileAtlas.log) {
                        return; // Có cây quá gần, không spawn
                    }
                }
            }

            // Kiểm tra nền (cần cỏ bên dưới)

            if (tileData[x, y - 1, 1] != tileAtlas.grass) return;


            for (int i = 0; i < height; i++) {
                PlaceTile(tileAtlas.log, x, y + i);

                if (i == 0) continue;
                int rChance = Random.Range(0, 10);
                int lChance = Random.Range(0, 10);

                //leafs
                if (rChance < 2) //spawn on the right
                    PlaceTile(tileAtlas.leaf, x + 1, y + i);
                if (lChance < 2) //spawn on left
                    PlaceTile(tileAtlas.leaf, x - 1, y + i);
            }

            //top_trees
            PlaceTile(tileAtlas.leaf, x, y + height);

            //roots

            int rRoot = Random.Range(0, 10);
            int lRoot = Random.Range(0, 10);

            if (rRoot < 6 && tileData[x + 1, y - 1, 1] == tileAtlas.grass && tileData[x + 1, y, 1] != tileAtlas.grass) {
                PlaceTile(tileAtlas.log, x + 1, y);
            }
            if (lRoot < 6 && tileData[x - 1, y - 1, 1] == tileAtlas.grass && tileData[x - 1, y, 1] != tileAtlas.grass) {
                PlaceTile(tileAtlas.log, x - 1, y);
            }


        }
        private void SpawnVine(int x, int y) //vine schematic
        {
            int length = Random.Range(4, 6);
            int i = 0;
            while (i < length && tileData[x, y - i, 1] is null)
            {
                PlaceTile(tileAtlas.vine, x, y - i);
                i++;
            }
        }
        public void PlaceTile(TileClass tile, int x, int y, bool updateLighting = false)
        {
            if (tile is ExtendedTileClass)
            {
                PlaceExtendedTile(tile as ExtendedTileClass, x, y);
                return;
            }

            if (x < 0 || x >= terrainSettings.worldSize.x) return;
            if (y < 0 || y >= terrainSettings.worldSize.y) return;
            if (!(GetTile((int)tile.tileLayer, x, y) is null)) return;

            tileData[x, y, (int)tile.tileLayer] = tile; //add to data
            tileMaps[(int)tile.tileLayer].SetTile(new Vector3Int(x, y, 0), tile.tile); //add to tilemap

            if (updateLighting) lighting.IUpdate();
            if (tile is LiquidTileClass)
            {
                LiquidTile waterTile = new LiquidTile(x, y, this, (LiquidTileClass)tile);
                 StartCoroutine(waterTile.CalculatePhysics());
               // waterTile.CalculatePhysics();
            }
        }
        private void PlaceExtendedTile(ExtendedTileClass tile, int x, int y)
        {
            int rootx = Mathf.Min(x, x + tile.size.x);
            int rooty = Mathf.Min(y, y + tile.size.y);
            int sizeX = Mathf.Abs(tile.size.x);
            int sizeY = Mathf.Abs(tile.size.y);
            if (rootx - 1 < 0 || rootx + 1 + sizeX >= terrainSettings.worldSize.x) return;
            if (rooty < 0 || rooty + sizeY >= terrainSettings.worldSize.y) return;
            
            for (int ix = rootx - 1; ix < rootx + sizeX + 1; ix++)
            {
                for (int iy = rooty; iy < rooty + sizeY; iy++)
                {
                    if (!(tileData[ix, iy, (int)tile.tileLayer] is null)) return;
                    if (terrainSettings.cavePoints[ix, iy]) return;
                }
            }

            for (int ix = rootx; ix < rootx + sizeX; ix++)
            {
                for (int iy = rooty; iy < rooty + sizeY; iy++)
                {
                    tileData[ix, iy, (int)tile.tileLayer] = tile; //add to data
                    tileMaps[(int)tile.tileLayer].SetTile(new Vector3Int(ix, iy, 0), tile.tile); //add to tilemap
                }
            }
        }
        public void RemoveTile(TileLayer layer, int x, int y, bool updateLighting = false)
        {
            if (x < 0 || x >= terrainSettings.worldSize.x) return;
            if (y < 0 || y >= terrainSettings.worldSize.y) return;
            if ((GetTile((int)layer, x, y) is null)) return;
            tileData[x, y, (int)layer] = null; //remove from data
            tileMaps[(int)layer].SetTile(new Vector3Int(x, y, 0), null); //remove from tileMap
            CheckAddOns(x, y);
            if (updateLighting) lighting.IUpdate();
        }
        private void CheckAddOns(int x, int y) //renove any addon tiles attached to this tile
        {
            //water
            if (tileData[x, y + 1, 3] is LiquidTileClass ||
                tileData[x + 1, y, 3] is LiquidTileClass ||
                tileData[x - 1, y, 3] is LiquidTileClass) 
            {
                if (!(tileData[x, y + 1, 3] is null))
                {
                    PlaceTile(tileData[x, y + 1, 3], x, y);
                    return;
                }
                if (!(tileData[x + 1, y, 3] is null))
                {
                    PlaceTile(tileData[x + 1, y, 3], x, y);
                    return;
                }
                if (!(tileData[x - 1, y, 3] is null))
                {
                    PlaceTile(tileData[x - 1, y, 3], x, y);
                    return;
                }
            }
            //attached
            if (!(tileData[x, y + 1, 0] is null))
                RemoveTile(TileLayer.addon, x, y + 1, false);
            if (!(tileData[x, y - 1, 0] is null))
                RemoveTile(TileLayer.addon, x, y - 1, false);
            if (!(tileData[x + 1, y, 0] is null))
                RemoveTile(TileLayer.addon, x + 1, y, false);
            if (!(tileData[x - 1, y, 0] is null))
                RemoveTile(TileLayer.addon, x - 1, y, false);
        }
        private TileClass GetTile(int layer, int x, int y) { return tileData[x, y, layer]; }
        public bool isIlluminate(int x, int y) //returns true if tile at this position illuminates light
        {
            for (int i = 0; i < tileData.GetLength(2); i++)
            {
                if (tileData[x, y, i] is null) continue;
                if (tileData[x, y, i].isIlluminate) return true;
            }
            return false;
        }
    }
}
