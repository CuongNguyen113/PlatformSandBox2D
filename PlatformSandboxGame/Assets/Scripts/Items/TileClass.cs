using UnityEngine;
using UnityEngine.Tilemaps;
using TerrariaClone.Enums;

namespace TerrariaClone.Items
{
    [CreateAssetMenu(fileName = "new Tile", menuName = "Resources/Items/Tile")]
    public class TileClass : Item
    {
        [field: Tooltip("Rule tile or image for this tile")]
        [field: SerializeField] public TileBase tile { get; private set; }
        [field: Tooltip("Layer in the world for this tile to spawn." +
            "\nAddon for trees, grass and similar" +
            "\nBackground for background wall tiles" +
            "\nGround = with collider" +
            "\nLiquid should only be for liquid Tiles")]
        [field: SerializeField] public TileLayer tileLayer { get; private set; }
        [field: Tooltip("Whether this tile emits light or not")]
        [field: SerializeField] public bool isIlluminate { get; private set; }
    }
}