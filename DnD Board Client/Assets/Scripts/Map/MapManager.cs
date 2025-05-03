using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataObjects.Units;
using DefaultNamespace;
using Map;
using Scriptable_Objects.Units.BaseUnits.Classes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager MapManagerInstance;
    private MapTileMapManager _tileMapManager;
    public SpriteRenderer mapSpriteRenderer;
    public Dictionary<string, BaseUnitController> unitControllers = new();
    public GameObject playerUnitContainer;
    public GameObject enemyUnitContainer;
    public Vector3 bottomLeftCorner { get; private set; }
    public GameObject currentlySelectedUnit;
    
    private Tile _previousTile;
    private string _currentlySelectedMapFileName;
    private Vector3Int _previousTilePosition;

    private bool _placeUnitMode;

    public GameObject unitPrefab;

    private string _unitName;
    private Vector3 MouseWorldPosition;
    private MapData mapData;
    public float unitSelectDelayThreshold = 1f;
    public float unitSelectDelay = 0f;
    
    private MapData _currentMapData;
    public void Awake()
    {
        MapManagerInstance = this;
    }

    public void StopPlaceUnitMode()
    {
        _placeUnitMode = false;
    }
    public void PlaceUnit(string unitName)
    {
        _placeUnitMode = true;
        //TODO MAKE A UNIT GALLARY
        _unitName = unitName;
    }
    public void Start()
    {
        _tileMapManager = MapTileMapManager.MapTileMapManagerInstance;
        CampaignLoader.Instance.LoadCampaignData("Test Campaign.json");
        MapUI.Instance.PopulateMapsDropDown(CampaignManager.Instance.mapFileNames);
        
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>().WorldToCell(mouseWorldPos);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var tileMapPos = _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>().WorldToCell(mouseWorldPos);
        return _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>().GetCellCenterWorld(tileMapPos);
    }
    private Tile GetTileAtMousePosition()
    {
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var tileMapPos = _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>().WorldToCell(mouseWorldPos);
        return _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>().GetTile<Tile>(tileMapPos);
    }

    public void DestroySelectedUnit()
    {
        Destroy(currentlySelectedUnit);
    }
    public void UpdateUnitMoveSpeed(int ms)
    {
        currentlySelectedUnit.GetComponent<BaseUnitController>().movementSpeed = ms;
    }
    private void Update()
    {
        if (unitSelectDelay < unitSelectDelayThreshold)
        {
            unitSelectDelay += Time.deltaTime;
        }

        MouseWorldPosition = GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(1) && _placeUnitMode)
        {

            var unitToPlace = Instantiate(unitPrefab);
            var controller = unitToPlace.GetComponent<BaseUnitController>();
            unitToPlace.name = _unitName;

            if (_unitName == "Enemy")
            {

                unitToPlace.transform.parent = enemyUnitContainer.transform;
                controller.BodyRenderer.color = new Color(1f, 0f, 0f, 1f);
                var enemyCount = enemyUnitContainer.transform.childCount;
                controller.namePlate.text = _unitName + enemyCount;
            }
            else
            {
                unitToPlace.transform.parent = playerUnitContainer.transform;
                controller.namePlate.text = _unitName;
                _placeUnitMode = false;
            }
            unitToPlace.transform.localScale = new Vector3(mapData.TileHeight, mapData.TileHeight, 1);
            unitToPlace.gameObject.transform.position = MouseWorldPosition;
            
            

        }
        
        
        
        if (currentlySelectedUnit != null && unitSelectDelay >= unitSelectDelayThreshold)
        {
            if (Input.GetMouseButtonDown(1))
            {
                currentlySelectedUnit.transform.position = GetMouseWorldPosition();
                currentlySelectedUnit.GetComponent<BaseUnitController>().UpdateVision();
                currentlySelectedUnit = null;
                
            }
        }
    }

    public void DiscoverTiles(Vector3Int unitPosition)
    {
        //TODO - MAKE THIS DYNAMIC 
        for (int i = 0; i < 12; i++)
        {
            //if(_tileMapManager.tileMaps["vision"].GetT)
        }
    }

    public void LoadMapFromFile(string mapFileName)
    {
        ClearAllTilemaps();
        var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client/Maps";
        var filePath = Path.Combine(documentsLocation, mapFileName + ".json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            mapData = JsonUtility.FromJson<MapData>(json);


            var sprite = Resources.Load<Sprite>(mapData.MapFileName);
            mapSpriteRenderer.sprite = sprite;
            mapSpriteRenderer.transform.position = new Vector3(0, 0, 5);

            bottomLeftCorner = mapSpriteRenderer.transform.TransformPoint(mapSpriteRenderer.sprite.bounds.min);

            _tileMapManager.LoadFromData(mapData);
            _tileMapManager.SnapToMapImage(bottomLeftCorner);
        }
    }

    void ClearAllTilemaps()
    {
        foreach (var tileMap in _tileMapManager.tileMaps)
        {
            var tm = tileMap.Value.GetComponent<Tilemap>();
            
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
    public void ChangeCurrentMap(string mapFileName)
    {
        
    }
    
}

