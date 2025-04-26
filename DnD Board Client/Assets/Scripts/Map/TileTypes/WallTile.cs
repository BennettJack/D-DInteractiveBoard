using Map.TileTypes;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Tiles/WallTile")]
public class WallTile : CustomTileBase
{
    public enum WallType
    {
        FullCover,
        HalfCover,
    }
    
    public WallType type { get; private set; }

    public void SetWallType(WallType type)
    {
        this.type = type;
    }
    
    
}
 