using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataObjects.Units;
using DefaultNamespace;
using DefaultNamespace.Commands;
using Map;
using Scriptable_Objects.Units.BaseUnits;
using Scriptable_Objects.Units.BaseUnits.Classes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager MapManagerInstance;
    private MapTileMapManager _tileMapManager;
    
    public GameObject playerUnitContainer;
    public GameObject enemyUnitContainer;
    public GameObject currentlySelectedUnit;
    public Dictionary<string, GameObject> placedEnemyUnits = new();
    public Dictionary<string, GameObject> placedPlayerUnits = new();
    private Tile _previousTile;
    private string _currentlySelectedMapFileName;
    private Vector3Int _previousTilePosition;

    public IBaseUnit unitToPlace;
    private bool _placeUnitMode;

    public GameObject unitPrefab;

    public List<GameObject> instantiatedPlayerUnits;
    public List<GameObject> instantiatedEnemyUnits;
    public MapData mapData;
    private Vector3 MouseWorldPosition;
    public float unitSelectDelayThreshold = 1f;
    public float unitSelectDelay = 0f;
    
    private MapData _currentMapData;
    public void Awake()
    {
        MapManagerInstance = this;
    }
    
    public void Start()
    {
        _tileMapManager = MapTileMapManager.MapTileMapManagerInstance;
        CampaignLoader.Instance.LoadCampaignData("Test Campaign.json");
        MapUI.Instance.PopulateMapsDropDown(CampaignManager.Instance.mapFileNames);
        
    }
    

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var tileMapPos = _tileMapManager.tileMaps["ground"].WorldToCell(mouseWorldPos);
        return _tileMapManager.tileMaps["ground"].GetCellCenterWorld(tileMapPos);
    }
    private Tile GetTileAtMousePosition()
    {
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var tileMapPos = _tileMapManager.tileMaps["ground"].WorldToCell(mouseWorldPos);
        return _tileMapManager.tileMaps["ground"].GetTile<Tile>(tileMapPos);
    }

    public void DestroySelectedUnit()
    {
        Destroy(currentlySelectedUnit);
    }
    private void Update()
    {
        if (unitSelectDelay < unitSelectDelayThreshold)
        {
            unitSelectDelay += Time.deltaTime;
        }

        MouseWorldPosition = GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(1) && unitToPlace is not null)
        {
            PlaceUnit();
        }
        
    }

    public void LoadMapFromFile(string mapFileName)
    {
        MapLoader.Instance.LoadMapFromFile(mapFileName);
    }

    
    public void ChangeCurrentMap(string mapFileName)
    {
        
    }

    public void SetUnitToPlace(string unitName)
    {
        unitToPlace = UnitManager.UnitManagerInstance.GetUnitByName(unitName);
    }

    private void PlaceUnit()
    {
        var unit = Instantiate(unitPrefab);
        unit.name = unitToPlace.unitName;
        unit.GetComponent<BaseUnitController>().BaseUnit = unitToPlace;

        unit.transform.localScale = new Vector3(mapData.TileWidth, mapData.TileHeight, 1);
        unit.gameObject.transform.position = MouseWorldPosition;

        if (CampaignManager.Instance.playerUnitNames.Contains(unitToPlace.unitName))
        {
            unit.GetComponent<BaseUnitController>().namePlate.text = unitToPlace.unitName;
            unit.GetComponent<BaseUnitController>().selectUnitOnMapCommand = new SelectPlayerUnitOnMapCommand();
            instantiatedPlayerUnits.Add(unit);
            unitToPlace = null;
        }
        else if (CampaignManager.Instance.enemyUnitNames.Contains(unitToPlace.unitName))
        {
            instantiatedEnemyUnits.Add(unit);
            unit.GetComponent<BaseUnitController>().BodyRenderer.color = Color.red;
            unit.GetComponent<BaseUnitController>().selectUnitOnMapCommand = new SelectEnemyUnitOnMapCommand();
            var unitsWithName = instantiatedEnemyUnits.Count(u => u.GetComponent<BaseUnitController>().BaseUnit.unitName == unitToPlace.unitName);
            if (unitsWithName == 1)
            {
                unit.GetComponent<BaseUnitController>().namePlate.text = unitToPlace.unitName;
            }
            else
            {
                unit.GetComponent<BaseUnitController>().namePlate.text = $"{unitToPlace.unitName} {unitsWithName - 1}";
            }
        }
        
        
        //TEMP CODE
        MovementManager.Instance.HighlightTilesInRangeDijkstra(_tileMapManager.tileMaps["ground"].WorldToCell(unit.transform.position), 6);
    }
}

