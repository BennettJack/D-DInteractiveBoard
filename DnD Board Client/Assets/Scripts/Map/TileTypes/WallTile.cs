using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Tiles/WallTile")]
public class WallTile : Tile
{
    public Vector3 position;
    public string test;

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
 