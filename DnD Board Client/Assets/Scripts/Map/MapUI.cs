using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Adapters;
using DataObjects.Units;
using DefaultNamespace;
using DefaultNamespace.CampaignSetup;
using DefaultNamespace.Commands;
using Map;
using Newtonsoft.Json;
using Scriptable_Objects.Units.BaseUnits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    
    private MapManager _mapManager;
    
    public Button PlaceUnitMode;
    public Button StopPlaceMode;
    public TMP_Dropdown MapSelection;
    
    
    public GameObject UnitSelectionPanel;
    public GameObject EnemyUnitInfo;
    public GameObject PlayerUnitInfo;

    public static MapUI Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _mapManager = MapManager.MapManagerInstance;
        
        MapSelection.onValueChanged.AddListener(delegate
        {
            ChangeMapSelection(MapSelection);
        });

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PopulateMapsDropDown(List<string> maps)
    {
        MapSelection.ClearOptions();
        MapSelection.options.Add(
            new TMP_Dropdown.OptionData(){text = "Please Select"});
        foreach (var map in maps)
        {
            MapSelection.options.Add(
                new TMP_Dropdown.OptionData(){text = map});
        }
        
        MapSelection.RefreshShownValue();
    }
    public void DisplayPlayerUnitList()
    {
        UnitSelectionPanel.SetActive(true);
        var controller = UnitSelectionPanel.GetComponent<UnitSelectorController>();
        var playerUnits = UnitManager.UnitManagerInstance.GetUnitByName(CampaignManager.Instance.playerUnitNames);
        var adaptedData = playerUnits.Select(u => (ICardData)new UnitDataCardAdapter(u)).ToList();
        controller.PopulateUnitList(adaptedData, new SelectUnitToPlaceCommand());
        
    }
    public void DisplayEnemyUnitsList()
    {
        
        UnitSelectionPanel.SetActive(true);
        var controller = UnitSelectionPanel.GetComponent<UnitSelectorController>();
        var enemyUnits = UnitManager.UnitManagerInstance.GetUnitByName(CampaignManager.Instance.enemyUnitNames);
        var adaptedData = enemyUnits.Select(u => (ICardData)new UnitDataCardAdapter(u)).ToList();
        controller.PopulateUnitList(adaptedData, new SelectUnitToPlaceCommand());
    }

    public void ChangeMapSelection(TMP_Dropdown mapSelection)
    {
        //-1 because there is a "please select" default option
        if (mapSelection.value != 0)
        {
            var selectedMap = CampaignManager.Instance.mapFileNames[mapSelection.value - 1];
            _mapManager.LoadMapFromFile(selectedMap);
        }
    }

    public void DisplayPlayerUnitInfo(BaseUnit selectedUnit)
    {
        if (UnitSelectionPanel.activeSelf)
        {
            UnitSelectionPanel.SetActive(false);
        }

        if (EnemyUnitInfo.activeSelf)
        {
            EnemyUnitInfo.SetActive(false);
        }
        
        PlayerUnitInfo.SetActive(true);
        PlayerUnitInfo.GetComponent<PlayerUnitInfoPanelUI>().UpdatePanel(selectedUnit);
    }

    public void DisplayEnemyUnitInfo(BaseUnit selectedUnit)
    {
        if (UnitSelectionPanel.activeSelf)
        {
            UnitSelectionPanel.SetActive(false);
        }

        if (EnemyUnitInfo.activeSelf)
        {
            PlayerUnitInfo.SetActive(false);
        }
        
        EnemyUnitInfo.SetActive(true);
        
    }
    public void DestroyUnit()
    {
        _mapManager.DestroySelectedUnit();
    }
}
