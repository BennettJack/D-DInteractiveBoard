using System.Collections.Generic;
using Map.TileTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/VisionTile")]
public class VisionTile : CustomTileBase
{
    public List<string> DiscoveredBy { get; private set; }

    public void AddToDiscoveredBy(string characterName)
    {
        DiscoveredBy.Add(characterName);
    }
}
