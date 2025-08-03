using System.Collections;
using TerrariaClone.Items;
using UnityEngine;

namespace TerrariaClone.Terrain
{
    public class LiquidTile
    {
        TerrainGenerator terrain;
        LiquidTileClass liquidData;
        int x;
        int y;

        public LiquidTile(int x, int y, TerrainGenerator terrain, LiquidTileClass liquidData)
        {
            this.x = x;
            this.y = y;
            this.terrain = terrain;
            this.liquidData = liquidData;
        }

        public IEnumerator CalculatePhysics()
        {
            yield return new WaitForSeconds(liquidData.flowSpeed);
            if (y > 0 &&
                terrain.tileData[x, y - 1, 1] is null)
                terrain.PlaceTile(liquidData, x, y - 1, liquidData.isIlluminate);
            if (x > 0 &&
                y > 0 &&
                terrain.tileData[x - 1, y, 1] is null && !(terrain.tileData[x, y - 1, 1] is null))
                terrain.PlaceTile(liquidData, x - 1, y, liquidData.isIlluminate);
            if (x < terrain.terrainSettings.worldSize.x &&
                y > 0 &&
                terrain.tileData[x + 1, y, 1] is null && !(terrain.tileData[x, y - 1, 1] is null))
                terrain.PlaceTile(liquidData, x + 1, y, liquidData.isIlluminate);
        }
    }
}
