using System;
using System.Collections.Generic;
using Map.TileTypes;
using TileMapControl;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class TileMapManager : MonoBehaviour
{
    public Dictionary<string, Tilemap> tileMaps { get; protected set; } = new();
    public Grid groundAndVisionGrid;
    public Grid wallGrid;
    protected TileGallery _tileGallery;
    protected TileMapGenerator _tileMapGenerator;
    
    
    public float tileWidth {get; protected set;}
    public float tileHeight {get; protected set;}
    public int horizontalTileCount { get; protected set; }
    public int verticalTileCount{ get; protected set; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    
    protected void AddTileMapsToDictionary()
    {
        tileMaps.Add("vision", GameObject.FindGameObjectWithTag("VisionTileMap").GetComponent<Tilemap>());
        tileMaps.Add("ground", GameObject.FindGameObjectWithTag("GroundTileMap").GetComponent<Tilemap>());
        tileMaps.Add("wall", GameObject.FindGameObjectWithTag("WallTileMap").GetComponent<Tilemap>());
        tileMaps.Add("overlay", GameObject.FindGameObjectWithTag("OverlayTileMap").GetComponent<Tilemap>());
    }
    public void SnapToMapImage(Vector3 position)
    {
        foreach (var tileMap in tileMaps)
        {
                tileMap.Value.transform.position = position;
        }
    }


    public void PlaceWallTile(Vector3Int position, WallTile.WallType type)
    {
        tileMaps["wall"].SetTile(position, _tileGallery.GetTile("WallTile"));
        tileMaps["wall"].SetTileFlags(position, TileFlags.None);
        tileMaps["wall"].SetColor(position, Color.blue);
        tileMaps["wall"].GetTile<WallTile>(position).SetWallType(type);

    }

    public void CreateNewTileSets()
    {
        
    }
    protected void CreateNewTileSet(string tileSetName)
    {
        for (int i = 0; i < verticalTileCount; i++)
        {
            for (int j = 0; j < horizontalTileCount; j++)
            {
                var tile = Instantiate(_tileGallery.GetTile("Preview"));
                if (tileSetName == "preview")
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            tile.color = Color.white;
                            tileMaps[tileSetName].SetTile(new Vector3Int(j, i, 0),
                                tile);
                        }
                        else
                        {
                            tile.color = Color.black;
                            tileMaps[tileSetName].SetTile(new Vector3Int(j, i, 0),
                               tile);
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            tile.color = Color.black;
                            tileMaps[tileSetName].SetTile(new Vector3Int(j, i, 0),
                                tile);

                        }
                        else
                        {
                            tile.color = Color.white;
                            tileMaps[tileSetName].SetTile(new Vector3Int(j, i, 0),
                                tile);


                        }
                    }
                }
                else
                {
                    tileMaps[tileSetName].SetTile(new Vector3Int(j, i, 0), _tileGallery.GetTile("Preview"));
                }

                tileMaps[tileSetName].SetTileFlags(new Vector3Int(j, i, 0), TileFlags.None);
            }
        }
    }

    
    
    public void ClearAllTilemaps()
    {
        foreach (var tileMap in tileMaps)
        {
            var tm = tileMap.Value;
            
            BoundsInt bounds = tm.cellBounds;

            foreach (var coord in bounds.allPositionsWithin)
            {
                if (tm.HasTile(coord))
                {
                    var tile = tm.GetInstantiatedObject(coord);
                    if (tile != null)
                    {
                        Debug.Log(tile.name);
                        Destroy(tile);
                    }

                    tm.SetTile(coord, null);
                }
            }
        }
    }
    
    protected void UpdateTileSize()
    {
        groundAndVisionGrid.transform.localScale = new Vector3(tileWidth, tileHeight, 0);
        wallGrid.transform.localScale = new Vector3(tileWidth / 2, tileHeight / 2, 0);
    }
    
    public void LoadFromData(MapData mapData, string sceneType)
    {

        this.tileHeight = mapData.TileHeight;
        this.tileWidth = mapData.TileWidth;
        this.horizontalTileCount = mapData.HorizontalTileCount;
        this.verticalTileCount = mapData.VerticalTileCount;
        foreach (var wall in mapData.WallTiles)
        {
            tileMaps["wall"].SetTile(wall.position, Resources.Load<WallTile>("Tiles/WallTiles/WallTile"));
            tileMaps["wall"].SetTileFlags(wall.position, TileFlags.None);
            tileMaps["wall"].SetColor(wall.position, Color.blue);
            var tile = tileMaps["wall"].GetTile<WallTile>(wall.position);
            tile.SetWallType(WallTile.WallType.FullCover);
            tile.colliderType = Tile.ColliderType.Grid;
        }

        _tileMapGenerator.GenerateVisionTileMap(horizontalTileCount, verticalTileCount,
            mapData, tileMaps["vision"]);
        //this is super jank, TODO - FIX
        if (sceneType == "map")
        {
            _tileMapGenerator.GenerateGroundTileMap(horizontalTileCount, verticalTileCount);
        }
        UpdateTileSize();
        
    }
    
    
}
