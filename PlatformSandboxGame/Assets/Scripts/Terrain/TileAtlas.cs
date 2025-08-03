using TerrariaClone.Items;
using UnityEngine;

namespace TerrariaClone.Terrain
{
    [CreateAssetMenu(fileName = "TileAtlas", menuName = "Resources/TileAtas")]
    public class TileAtlas : ScriptableObject
    {
        //references to all the tiles our terrain generate uses to generate the world
        [field: SerializeField] public TileClass grass { get; private set; }
        [field: SerializeField] public TileClass dirt { get; private set; }
        [field: SerializeField] public TileClass stone { get; private set; }
        [field: SerializeField] public TileClass dirtWall { get; private set; }
        [field: SerializeField] public TileClass stoneWall { get; private set; }
        [field: SerializeField] public TileClass tallGrass { get; private set; }
        [field: SerializeField] public TileClass log { get; private set; }
        [field: SerializeField] public TileClass leaf { get; private set; }
        [field: SerializeField] public TileClass water { get; private set; }
        [field: SerializeField] public TileClass lava { get; private set; }
        [field: SerializeField] public TileClass vine { get; private set; }
        [field: SerializeField] public TileClass stalactite { get; private set; }
        [field: SerializeField] public TileClass pot { get; private set; }
        [field: SerializeField] public TileClass torch { get; private set; }
    }
}
