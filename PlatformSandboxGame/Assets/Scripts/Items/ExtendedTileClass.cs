using UnityEngine;
using UnityEngine.Tilemaps;

namespace TerrariaClone.Items
{
    [CreateAssetMenu(fileName = "new Tile", menuName = "Resources/Items/Extended Tile")]
    public class ExtendedTileClass : TileClass
    {
        public Vector2Int size;
    }
}
