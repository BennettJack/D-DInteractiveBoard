using System;
using System.Collections.Generic;
using Map.TileTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGallery : MonoBehaviour
{
    private Dictionary<string, CustomTileBase> _tiles;


    public static TileGallery TileGalleryInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        TileGalleryInstance = this;
        
        _tiles = new Dictionary<string, CustomTileBase>();

        
        _tiles.Add("Black", Resources.Load<CustomTileBase>("Tiles/Black"));
        _tiles.Add("White", Resources.Load<CustomTileBase>("Tiles/White"));
        
        _tiles.Add("NoVision", Resources.Load<VisionTile>("Tiles/VisionTiles/NoVision"));
        _tiles.Add("FullVision", Resources.Load<VisionTile>("Tiles/VisionTiles/FullVision"));
        _tiles.Add("WallTile", Resources.Load<WallTile>("Tiles/WallTiles/WallTile"));
    }

    public CustomTileBase GetTile(string key)
    {
        return _tiles[key];
    }

}
