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
    public Dictionary<string, GameObject> tileMaps { get; protected set; } = new();
    public Grid groundAndVisionGrid;
    public Grid wallGrid;
    protected TileGallery _tileGallery;
    
    public static TileMapManager TileMapManagerInstance;
    
    protected TileMapGenerator _tileMapGenerator;
    public float tileWidth {get; protected set;}
    public float tileHeight {get; protected set;}
    public int horizontalTileCount { get; protected set; }
    public int verticalTileCount{ get; protected set; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    protected void AddTileMapsToDictionary()
    {
        tileMaps.Add("vision", GameObject.FindGameObjectWithTag("VisionTileMap"));
        tileMaps.Add("ground", GameObject.FindGameObjectWithTag("GroundTileMap"));
        tileMaps.Add("wall", GameObject.FindGameObjectWithTag("WallTileMap"));
        tileMaps.Add("overlay", GameObject.FindGameObjectWithTag("OverlayTileMap"));
    }
    public void SnapToMapImage(Vector3 position)
    {
        //TODO FIX THIS IT'S SO WONKY
        foreach (var tileMap in tileMaps)
        {
                tileMap.Value.transform.position = position;
        }
    }


    public void PlaceWallTile(Vector3Int position, WallTile.WallType type)
    {
        tileMaps["wall"].GetComponent<Tilemap>().SetTile(position, _tileGallery.GetTile("WallTile"));
        tileMaps["wall"].GetComponent<Tilemap>().SetTileFlags(position, TileFlags.None);
        tileMaps["wall"].GetComponent<Tilemap>().SetColor(position, Color.blue);
        tileMaps["wall"].GetComponent<Tilemap>().GetTile<WallTile>(position).SetWallType(type);

    }
    protected void CreateNewTileSet(string tileSetName)
    {
        for (int i = 0; i < verticalTileCount; i++)
        {
            for (int j = 0; j < horizontalTileCount; j++)
            {
                if (tileSetName == "preview")
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("Preview"));
                        }
                        else
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("Preview"));
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("Preview"));

                        }
                        else
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("Preview"));


                        }
                    }
                }
                else if (tileSetName == "vision")
                {
                    var tile = Instantiate(_tileGallery.GetTile("NoVision"));
                    tile.position = new Vector3Int(j, i, 0);
                    tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0), tile);
                }
                else
                {
                    Debug.Log(_tileGallery.name);
                    tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0), _tileGallery.GetTile("Preview"));
                }

                tileMaps[tileSetName].GetComponent<Tilemap>().SetTileFlags(new Vector3Int(j, i, 0), TileFlags.None);
            }
        }
    }

    private void SetNeighbours()
    {
        foreach (var tileMap in tileMaps)
        {
            if (tileMap.Key == "vision")
            {
                for (int i = 0; i < verticalTileCount; i++)
                {
                    for (int j = 0; j < horizontalTileCount; j++)
                    {
                        var tile = tileMap.Value.GetComponent<Tilemap>().GetTile<CustomTileBase>(new Vector3Int(j, i, 0));
                        if (i - 1 > 0)
                        {
                            tile.Neighbors.Add(tileMap.Value.GetComponent<Tilemap>().GetTile<CustomTileBase>(new Vector3Int(j, i - 1, 0)));
                        }

                        if (i + 1 < horizontalTileCount)
                        {
                            tile.Neighbors.Add(tileMap.Value.GetComponent<Tilemap>().GetTile<CustomTileBase>(new Vector3Int(j, i + 1, 0)));
                        }
                        
                        if (j - 1 > 0)
                        {
                            tile.Neighbors.Add(tileMap.Value.GetComponent<Tilemap>().GetTile<CustomTileBase>(new Vector3Int(j - 1, 0)));
                        }

                        if (j + 1 < horizontalTileCount)
                        {
                            tile.Neighbors.Add(tileMap.Value.GetComponent<Tilemap>().GetTile<CustomTileBase>(new Vector3Int(j + 1, i, 0)));
                        }
                    }
                }
            }
        }
    }
    
    protected void UpdateTileSize()
    {
        groundAndVisionGrid.transform.localScale = new Vector3(tileWidth, tileHeight, 0);
        wallGrid.transform.localScale = new Vector3(tileWidth / 2, tileHeight / 2, 0);
    }
    
    public void LoadFromData(MapData mapData)
    {

        this.tileHeight = mapData.TileHeight;
        this.tileWidth = mapData.TileWidth;
        this.horizontalTileCount = mapData.HorizontalTileCount;
        this.verticalTileCount = mapData.VerticalTileCount;
        foreach (var wall in mapData.WallTiles)
        {
            tileMaps["wall"].GetComponent<Tilemap>().SetTile(wall.position, Resources.Load<WallTile>("Tiles/WallTiles/WallTile"));
            tileMaps["wall"].GetComponent<Tilemap>().SetTileFlags(wall.position, TileFlags.None);
            tileMaps["wall"].GetComponent<Tilemap>().SetColor(wall.position, Color.blue);
            var tile = tileMaps["wall"].GetComponent<Tilemap>().GetTile<WallTile>(wall.position);
            tile.SetWallType(WallTile.WallType.FullCover);
            tile.colliderType = Tile.ColliderType.Grid;
        }
        
        CreateNewTileSet("ground");
        CreateNewTileSet("vision");
        UpdateTileSize();
        SetNeighbours();
        
    }
}
