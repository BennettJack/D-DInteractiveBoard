using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataObjects.Units;
using DefaultNamespace;
using DefaultNamespace.Commands;
using DefaultNamespace.TurnBasedScripts;
using Map;
using Scriptable_Objects.Units.BaseUnits;
using Scriptable_Objects.Units.BaseUnits.Classes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager MapManagerInstance;
    private MapTileMapManager _tileMapManager;
    
    public GameObject playerUnitContainer;
    public GameObject enemyUnitContainer;
    public GameObject currentlySelectedUnit;
    private Tile _previousTile;
    private string _currentlySelectedMapFileName;
    private Vector3Int _previousTilePosition;

    public IBaseUnit unitToPlace;
    private bool _placeUnitMode;

    public GameObject unitPrefab;

    public Dictionary<string, GameObject> instantiatedPlayerUnits = new();
    public Dictionary<string, GameObject> instantiatedEnemyUnits = new();
    public Dictionary<string, GameObject> allInstntiatedUnits = new();
    public MapData mapData;
    private Vector3 MouseWorldPosition;
    
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

    public Vector3Int GetUnitPosition(string unitName)
    {
        var units = new Dictionary<string, GameObject>();
        units.AddRange(instantiatedEnemyUnits);
        units.AddRange(instantiatedPlayerUnits);
        
        return _tileMapManager.tileMaps["ground"].WorldToCell(units[unitName].transform.position);
    }

    public void MoveUnit(Vector3 position, string unitName)
    {
        allInstntiatedUnits.TryGetValue(unitName, out var unit);
        if (!unit) return;
        
        unit.transform.position = position;
        
        
    }
    public void DestroySelectedUnit()
    {
        if(!currentlySelectedUnit) return;
        
        Destroy(currentlySelectedUnit);
        instantiatedPlayerUnits.Remove(currentlySelectedUnit.name);
        instantiatedEnemyUnits.Remove(currentlySelectedUnit.name);

        if (TurnBasedModeManager.Instance.IsTurnBasedMode)
        {
            TurnBasedModeManager.Instance.RemoveUnitFromInitiative(currentlySelectedUnit.name);
        };
    }
    private void Update()
    {

        MouseWorldPosition = GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(1) && unitToPlace is not null)
        {
            PlaceUnit();
        }

        foreach (var player in instantiatedPlayerUnits)
        {
            VisionManager.Instance.ClearVision(player.Value.transform.position);    
        }
        
    }

    public void LoadMapFromFile(string mapFileName)
    {
        MapLoader.Instance.LoadMapFromFile(mapFileName);
    }
    

    public void SetUnitToPlace(string unitName)
    {
        unitToPlace = UnitManager.UnitManagerInstance.GetUnitByName(unitName);
    }

    private void PlaceUnit()
    {
        var unit = Instantiate(unitPrefab);
        
        var unitController = unit.GetComponent<BaseUnitController>();
        unitController.BaseUnit = unitToPlace;
        unit.transform.localScale = new Vector3(mapData.TileWidth, mapData.TileHeight, 1);
        unit.gameObject.transform.position = MouseWorldPosition;
        
        
        if (CampaignManager.Instance.playerUnitNames.Contains(unitToPlace.unitName))
        {
            unit.name = unitToPlace.unitName;
            unitController.namePlate.text = unitToPlace.unitName;
            unitController.selectUnitOnMapCommand = new SelectPlayerUnitOnMapCommand();
            unit.transform.SetParent(playerUnitContainer.transform);
            instantiatedPlayerUnits.Add(unit.name, unit);
            unitToPlace = null;
        }
        else if (CampaignManager.Instance.enemyUnitNames.Contains(unitToPlace.unitName))
        {
            
            unitController.BodyRenderer.color = Color.red;
            unitController.selectUnitOnMapCommand = new SelectEnemyUnitOnMapCommand();
            unit.transform.SetParent(enemyUnitContainer.transform);
            var unitsWithName = instantiatedEnemyUnits.Count(u => u.Value.GetComponent<BaseUnitController>().BaseUnit.unitName == unitToPlace.unitName);
            if (unitsWithName == 0)
            {
                unit.name = unitToPlace.unitName;
                unitController.namePlate.text = unitToPlace.unitName;
            }
            else
            {
                unit.name = $"{unitToPlace.unitName} {unitsWithName + 1}";
                unitController.namePlate.text = $"{unitToPlace.unitName} {unitsWithName + 1}";
            }
            
            instantiatedEnemyUnits.Add(unit.name, unit);
            unitToPlace = null;
        }
        allInstntiatedUnits.Add(unit.name, unit);
        
        //TEMP CODE
        //var test = MovementManager.Instance.GetReachableTiles(_tileMapManager.tileMaps["ground"].WorldToCell(unit.transform.position), 6);
    }
}

