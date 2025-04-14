using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map.TileTypes
{
    public class CustomTileBase : Tile
    {
        public List<CustomTileBase> Neighbors = new List<CustomTileBase>();
        public Vector3Int position;
    }
}