using UnityEngine;

namespace TerrariaClone.Items
{
    [CreateAssetMenu(fileName = "liquid tile", menuName = "Resources/Items/LiquidTile")]
    public class LiquidTileClass : TileClass
    {
        [field: Tooltip("Speed in secs the liquid flows")]
        [field: SerializeField] public float flowSpeed { get; private set; }
    }
}
