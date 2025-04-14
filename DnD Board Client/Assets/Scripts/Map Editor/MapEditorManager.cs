using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEditorManager : MonoBehaviour
{
    public static MapEditorManager MapEditorManagerInstance;
    private TileMapManager _tileMapManager;

    public SpriteRenderer mapSpriteRenderer;
    public string mapName { get; private set; }
    public Vector3 bottomLeftCorner { get; private set; }

    public enum EditModeType
    {
        Setup,
        Wall,
        GroundTiles,
        Units
    }

    public EditModeType editMode;
    private void Awake()
    {
        MapEditorManagerInstance = this;
        editMode = EditModeType.Setup;
    }
    
    private void Start()
    {
        _tileMapManager = TileMapManager.TileMapManagerInstance;
    }
    
    public void SetTileCounts(int verticalTileCount, int horizontalTileCount)
    {
        _tileMapManager.SetTileCounts(verticalTileCount, horizontalTileCount);
        
        AutoSetTileSize();

    }

    private void AutoSetTileSize()
    {
        var spriteWidth = mapSpriteRenderer.sprite.bounds.size.x;
        var spriteHeight = mapSpriteRenderer.sprite.bounds.size.y;
        
        var tileWidth = spriteWidth / _tileMapManager.horizontalTileCount;
        var tileHeight = spriteHeight / _tileMapManager.verticalTileCount;
        
        SetTileHeight(tileHeight);
        SetTileWidth(tileWidth);
        
        _tileMapManager.SnapToMapImage(bottomLeftCorner);
    }
    
    
    public void SetTileHeight(float height)
    {
        _tileMapManager.SetTileHeight(height);
    }

    public void SetTileWidth(float width)
    {
        _tileMapManager.SetTileWidth(width);
    }
    
    public void SetMapImage(string filePath)
    {
        var sprite = Resources.Load<Sprite>(filePath);
        mapSpriteRenderer.sprite = sprite;
        mapSpriteRenderer.transform.position = new Vector3(0, 0, 5);
        
        bottomLeftCorner = mapSpriteRenderer.transform.TransformPoint(mapSpriteRenderer.sprite.bounds.min);
    }

    public bool TileMapHasTiles()
    {
        if (_tileMapManager.horizontalTileCount != 0 && _tileMapManager.verticalTileCount != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EnableWallPaintMode()
    {
        _tileMapManager.SetCurrentTileMapToEdit("wall");
        editMode = EditModeType.Wall;
    }


    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _tileMapManager.tileMaps["wall"].GetComponent<Tilemap>().WorldToCell(mouseWorldPos);
    }

    private void Update()
    {
        var mousePosition = GetMousePosition();
        if (editMode == EditModeType.Wall)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"should be placing wall at {mousePosition}");
                var wallType = WallTile.WallType.FullCover;
                _tileMapManager.PlaceWallTile(mousePosition, wallType);
            } 
        }
        
    }

    public void SetMapName(string mapName)
    {
        Debug.Log($"Set Map Name: {mapName}");
        this.mapName = mapName;
    }

    public void SaveMap()
    {
        var map = new MapData();
        map.MapFileName = "Maps/dotmm";
        map.TileWidth = _tileMapManager.tileWidth;
        map.TileHeight = _tileMapManager.tileHeight;
        
        map.HorizontalTileCount = _tileMapManager.horizontalTileCount;
        map.VerticalTileCount = _tileMapManager.verticalTileCount;
        
        var bounds = _tileMapManager.tileMaps["wall"].GetComponent<Tilemap>().cellBounds;
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                WallTile tile = _tileMapManager.tileMaps["wall"].GetComponent<Tilemap>().GetTile<WallTile>(pos);
                
                if (tile != null)
                {
                    map.WallTiles.Add(
                        new()
                        {

                            position = new Vector3Int(x, y, bounds.z),
                            wallType = tile.type.ToString()

                        });
                }
            }
        }
        
        Debug.Log(JsonUtility.ToJson(map, true));
        
        var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client/Maps";
        var filePath = Path.Combine(documentsLocation, mapName + ".json");

        var json = JsonUtility.ToJson(map, true);
        Debug.Log(json);

        if (!Directory.Exists(documentsLocation ))
        {
         Directory.CreateDirectory(documentsLocation);
        }

        if (File.Exists(filePath))
        {
         File.Delete(filePath);
         File.WriteAllText(filePath, json);
        }
        else
        {
         File.WriteAllText(filePath, json);
        }
    }
}
