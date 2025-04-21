using System.Collections.Generic;
using Map.TileTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/VisionTile")]
public class VisionTile : CustomTileBase
{
    public List<string> DiscoveredBy { get; private set; }
    public GameObject prefab;
    
    public void AddToDiscoveredBy(string characterName)
    {
        DiscoveredBy.Add(characterName);
    }
    
    public override bool StartUp(Vector3Int pos, ITilemap tilemap, GameObject go)
    {
        if (prefab != null && go == null)
        {
            Vector3 worldPos = tilemap.GetComponent<Tilemap>().GetCellCenterWorld(pos);
            GameObject newTile = Object.Instantiate(prefab, worldPos, Quaternion.identity);
            newTile.transform.SetParent(GameObject.Find("VisionCollisionTiles").transform);
            newTile.name = $"Tile_{pos}";
        }

        return base.StartUp(position, tilemap, go);
    }
}
