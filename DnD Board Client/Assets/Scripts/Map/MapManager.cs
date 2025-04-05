using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager MapManagerInstance;
    private TileMapManager _tileMapManager;
    public SpriteRenderer mapSpriteRenderer;
    public Dictionary<string, BaseUnitController> unitControllers = new();
    public GameObject playerUnitContainer;
    public GameObject enemyUnitContainer;
    public Vector3 bottomLeftCorner { get; private set; }
    
    public GameObject currentlySelectedUnit;
    
    private Tile _previousTile;

    private Vector3Int _previousTilePosition;

    private bool _placeUnitMode;

    public GameObject unitPrefab;

    private string _unitName;

    private Vector3 MouseWorldPosition;

    public float unitSelectDelayThreshold = 1f;
    public float unitSelectDelay = 0f;
    public void Awake()
    {
        MapManagerInstance = this;
        //TODO Clean up
        /*unitControllers.Add(
            "Jack",
            I
            {
                name = "Jack",
            });
        unitControllers.Add(
            "Beth",
            new()
            {
                name = "Beth",
            });
        unitControllers.Add(
            "Harry",
            new()
            {
                name = "Harry",
            });

        unitControllers["jack"].baseUnit.SetUnitName("jack");
        unitControllers["beth"].baseUnit.SetUnitName("beth");
        unitControllers["harry"].baseUnit.SetUnitName("harry");
        
        unitControllers["jack"].baseUnit.SetMoveSpeed(25);
        unitControllers["beth"].baseUnit.SetMoveSpeed(30);
        unitControllers["harry"].baseUnit.SetMoveSpeed(30);*/

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
        _tileMapManager = TileMapManager.TileMapManagerInstance;
        LoadMapFromFile();
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
        /*var tilePos = GetMousePosition();
        if (_previousTilePosition != tilePos)
        {
            Debug.Log("new tile");
            _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>()
                .GetTile<Tile>(_previousTilePosition).color = new Color(255, 255, 255, 0.0f);
            _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>()
                .GetTile<Tile>(tilePos).color = new Color(0, 255, 0, 0.4f);
            _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>()
                .RefreshTile(_previousTilePosition);
            _tileMapManager.tileMaps["ground"].GetComponent<Tilemap>()
                .RefreshTile(tilePos);
            _previousTilePosition = tilePos;
        }*/

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
                
            }

            unitToPlace.gameObject.transform.position = MouseWorldPosition;
            _placeUnitMode = false;
            

        }
        
        if (currentlySelectedUnit != null && unitSelectDelay >= unitSelectDelayThreshold)
        {
            Debug.Log(currentlySelectedUnit.name);
            if (Input.GetMouseButtonDown(1))
            {
                currentlySelectedUnit.transform.position = GetMouseWorldPosition();
                currentlySelectedUnit = null;
            }
        }
    }

    public void LoadMapFromFile()
    {
        var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client/Maps";
        var filePath = Path.Combine(documentsLocation, "IMG_4164.json");

        var json = File.ReadAllText(filePath);
        var mapData = JsonUtility.FromJson<MapData>(json);
        
        
        var sprite = Resources.Load<Sprite>(mapData.MapFileName);
        mapSpriteRenderer.sprite = sprite;
        mapSpriteRenderer.transform.position = new Vector3(0, 0, 5);
        
        bottomLeftCorner = mapSpriteRenderer.transform.TransformPoint(mapSpriteRenderer.sprite.bounds.min);
        
        _tileMapManager.LoadFromData(mapData);
        _tileMapManager.SnapToMapImage(bottomLeftCorner);
    }
    
}

