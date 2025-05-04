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

        
        _tiles.Add("Preview", Resources.Load<PreviewTile>("Tiles/PreviewTile"));
        _tiles.Add("NoVision", Resources.Load<VisionTile>("Tiles/VisionTiles/NoVision"));
        _tiles.Add("FullVision", Resources.Load<VisionTile>("Tiles/VisionTiles/FullVision"));
        _tiles.Add("WallTile", Resources.Load<WallTile>("Tiles/WallTiles/WallTile"));
        _tiles.Add("MovementOverlay", Resources.Load<MovementTile>("Tiles/OverlayTiles/MovementOverlayTile"));
        _tiles.Add("BlockedOverlay", Resources.Load<MovementTile>("Tiles/OverlayTiles/BlockedMovement"));
        _tiles.Add("standardTerrain", Resources.Load<FloorTile>("Tiles/FloorTiles/StandardTerrain"));
        _tiles.Add("difficultTerrain", Resources.Load<FloorTile>("Tiles/FloorTiles/DifficultTerrain"));
    }

    public CustomTileBase GetTile(string key)
    {
        return _tiles[key];
    }

}
