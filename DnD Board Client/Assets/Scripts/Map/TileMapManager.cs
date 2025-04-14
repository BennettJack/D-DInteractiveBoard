using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public Dictionary<string, GameObject> tileMaps { get; private set; } = new();
    public Grid groundAndVisionGrid;
    public Grid wallGrid;
    private TileGallery _tileGallery;
    
    public static TileMapManager TileMapManagerInstance;
    public float tileWidth {get; private set;}
    public float tileHeight {get; private set;}
    public int horizontalTileCount { get; private set; }
    public int verticalTileCount{ get; private set; }

    private void Awake()
    {
        TileMapManagerInstance = this;
        tileMaps.Add("vision", GameObject.FindGameObjectWithTag("VisionTileMap"));
        tileMaps.Add("ground", GameObject.FindGameObjectWithTag("GroundTileMap"));
        tileMaps.Add("wall", GameObject.FindGameObjectWithTag("WallTileMap"));
        tileMaps.Add("overlay", GameObject.FindGameObjectWithTag("OverlayTileMap"));
        tileMaps.Add("preview", GameObject.FindGameObjectWithTag("EditorPreviewTileMap"));

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            tileMaps["preview"].GetComponent<Tilemap>().color = new Color(255, 255, 255, 0.2f);
        }
        
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tileGallery = TileGallery.TileGalleryInstance;

    }

    public void SetCurrentTileMapToEdit(string tileMapName)
    {
        foreach (var tileMap in tileMaps)
        {
            if (tileMap.Key == tileMapName)
            {
                tileMap.Value.SetActive(true);
                tileMap.Value.GetComponent<Tilemap>().color = new Color(255, 255, 255, 0.2f);
            }
            else
            {
                tileMap.Value.SetActive(false);
            }
            
        }
    }

    public void SetTileWidth(float tWidth)
    {
        tileWidth = tWidth;
    
        //TODO - FIX THIS IT'S SUPER JANKY, IF YOU REMOVE SET WALL SIZE, EVERYTHING BREAKS
        UpdateTileSize();
        SetWallTileSize();
    }

    public void SetTileHeight(float tHeight)
    {
        tileHeight = tHeight;

        //TODO - FIX THIS IT'S SUPER JANKY
        UpdateTileSize();
        SetWallTileSize();
    }

    public void SnapToMapImage(Vector3 position)
    {
        //TODO FIX THIS IT'S SO WONKY
        foreach (var tileMap in tileMaps)
        {
            if (tileMap.Key != "preview")
            {
                tileMap.Value.transform.position = position;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 0 && tileMap.Key.Equals("preview"))
            {
                tileMap.Value.transform.position = position;
            }
        }
    }
    public void SetTileCounts(int vTileCount, int hTileCount)
    {
        verticalTileCount = vTileCount;
        horizontalTileCount = hTileCount;
        foreach (var tileMap in tileMaps)
        {
            tileMap.Value.GetComponent<Tilemap>().ClearAllTiles();
        }
        CreateNewTileSet("preview", verticalTileCount, horizontalTileCount);
    }

    private void SetWallTileSize()
    {
        wallGrid.transform.localScale = new Vector3(tileWidth / 2, tileHeight / 2, 0);
    }
    private void UpdateTileSize()
    {
        groundAndVisionGrid.transform.localScale = new Vector3(tileWidth, tileHeight, 0);
    }

    public void PlaceWallTile(Vector3Int position, WallTile.WallType type)
    {
        tileMaps["wall"].GetComponent<Tilemap>().SetTile(position, Resources.Load<WallTile>("Tiles/WallTiles/WallTile"));
        tileMaps["wall"].GetComponent<Tilemap>().SetTileFlags(position, TileFlags.None);
        tileMaps["wall"].GetComponent<Tilemap>().SetColor(position, Color.blue);
        tileMaps["wall"].GetComponent<Tilemap>().GetTile<WallTile>(position).SetWallType(type);
        //tileMaps["wall"].GetComponent<Tilemap>().GetTile<WallTile>(position).position = position;

    }
    private void CreateNewTileSet(string tileSetName, int vTileCount, int hTileCount)
    {
        for (int i = 0; i < vTileCount; i++)
        {
            for (int j = 0; j < hTileCount; j++)
            {
                if (tileSetName == "preview")
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("Black"));
                        }
                        else
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("White"));
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("White"));

                        }
                        else
                        {
                            tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0),
                                _tileGallery.GetTile("Black"));


                        }
                    }
                }
                else
                {
                    tileMaps[tileSetName].GetComponent<Tilemap>().SetTile(new Vector3Int(j, i, 0), _tileGallery.GetTile("White"));
                }

                tileMaps[tileSetName].GetComponent<Tilemap>().SetTileFlags(new Vector3Int(j, i, 0), TileFlags.None);
                if (tileSetName != "preview")
                {
                    tileMaps[tileSetName].GetComponent<Tilemap>().GetTile<Tile>(new Vector3Int(j, i, 0)).color = new Color(255, 255, 255, 0.0f);
                }
            }
        }
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
        
        CreateNewTileSet("ground", verticalTileCount, horizontalTileCount);
        SetWallTileSize();
        UpdateTileSize();
        
    }
}
