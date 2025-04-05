using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGallery : MonoBehaviour
{
    private Dictionary<string, Tile> _tiles;
    private Dictionary<string, WallTile> _wallTiles;

    public static TileGallery TileGalleryInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        TileGalleryInstance = this;
        
        _tiles = new Dictionary<string, Tile>();
        _wallTiles = new Dictionary<string, WallTile>();
        
        _tiles.Add("Black", (Tile)Resources.Load("Tiles/Black"));
        _tiles.Add("White", (Tile)Resources.Load("Tiles/White"));
    }

    public Tile GetTile(string key)
    {
        return _tiles[key];
    }

    public WallTile GetWallTile(string key)
    {
        return _wallTiles[key];
    }
    public void Test()
    {
        Debug.Log("test");
    }
}
