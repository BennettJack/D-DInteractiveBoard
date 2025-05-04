using Map.TileTypes;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Tiles/WallTile")]
public class WallTile : CustomTileBase
{
    public GameObject wallPrefab;
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
    
    public override bool StartUp(Vector3Int pos, ITilemap tilemap, GameObject go)
   {
      
       Tilemap wallTilemap = tilemap.GetComponent<Tilemap>();
       if (wallTilemap == null || wallPrefab == null) return base.StartUp(pos, tilemap, go);

       // Get world position for tile center
       Vector3 worldPos = wallTilemap.GetCellCenterWorld(pos);

       // Instantiate wall prefab at tile location
       GameObject wallObj = Instantiate(wallPrefab, worldPos, Quaternion.identity);

       // OPTIONAL: adjust size to match tile if needed
       BoxCollider2D col = wallObj.GetComponent<BoxCollider2D>();
       if (col != null)
       {
           col.size = wallTilemap.cellSize;
       }

       // OPTIONAL: assign to a parent GameObject for organization
       GameObject container = GameObject.Find("WallTileColliderContainer");
       if (container != null)
       {
           wallObj.transform.SetParent(container.transform);
       }

       return base.StartUp(pos, tilemap, go);
   }
    
    
}
 