

using System;
using System.Collections.Generic;
using Map.TileTypes.SerializableTiles;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

[Serializable]
public class MapData
{
    public float TileWidth;
    public float TileHeight;
    public int HorizontalTileCount;
    public int VerticalTileCount;
    public List<SerializableWallTile> WallTiles = new();
    public List<SerializableFloorTile> GroundTiles = new();
    public List<SerializableVisionTile> DiscoveredVisionTiles = new();
    public string MapFileName;
    private List<BaseUnit> Units;
}
