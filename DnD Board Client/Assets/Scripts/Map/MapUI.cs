using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DataObjects.Adapters;
using DataObjects.Units;
using DefaultNamespace;
using DefaultNamespace.CampaignSetup;
using DefaultNamespace.Commands;
using DefaultNamespace.TurnBasedScripts;
using Map_Editor;
using Map;
using Newtonsoft.Json;
using Scriptable_Objects.Units.BaseUnits;
using TMPro;
using Unity.VisualScripting;
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

    public GameObject initiativeButton;
    public GameObject InitiativePanel;
    private Dictionary<string, TMP_InputField> _unitInputFields = new();
    public GameObject InitiativePrefab;

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
        if (TurnBasedModeManager.Instance.IsTurnBasedMode)
        {
            initiativeButton.SetActive(false);
        }
        else
        {
            initiativeButton.SetActive(true);
        }
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

    public void DisplayInitiativePanel()
    {
        InitiativePanel.SetActive(true);
        UnitSelectionPanel.SetActive(false);
        PlayerUnitInfo.SetActive(false);
        EnemyUnitInfo.SetActive(false);
        foreach (var prefab in _unitInputFields.Values)
        {
            Destroy(prefab.transform.parent.gameObject);
        }
        _unitInputFields.Clear();
        var units = new Dictionary<string, GameObject>();
        units.AddRange(MapManager.MapManagerInstance.instantiatedEnemyUnits);
        units.AddRange(MapManager.MapManagerInstance.instantiatedPlayerUnits);
        foreach (var unit in units)
        {
            string unitName = unit.Key;
            GameObject input = Instantiate(InitiativePrefab, InitiativePanel.transform);
            var text = input.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = unit.Key;
            var inputField = input.transform.Find("InitiativeInputField").GetComponent<TMP_InputField>();
            _unitInputFields[unitName] = inputField;
        }
    }

    public void OnInitiativeSubmit()
    {
        Dictionary<string, int> unitValues = new Dictionary<string, int>();

        foreach (var kvp in _unitInputFields)
        {
            string unitName = kvp.Key;
            TMP_InputField input = kvp.Value;

            if (int.TryParse(input.text, out int value))
            {
                unitValues[unitName] = value;
            }
            else
            {
                Debug.LogWarning($"Invalid input for {unitName}");
                unitValues[unitName] = 0; // or handle differently
            }
        }

        TurnBasedModeManager.Instance.SetInitiative(unitValues);
        TurnBasedModeManager.Instance.StartCombat();
    }
    public void DisplayPlayerUnitList()
    {
        UnitSelectionPanel.SetActive(true);
        InitiativePanel.SetActive(false);
        var controller = UnitSelectionPanel.GetComponent<UnitSelectorController>();
        var playerUnits = UnitManager.UnitManagerInstance.GetUnitByName(CampaignManager.Instance.playerUnitNames);
        var adaptedData = playerUnits.Select(u => (ICardData)new UnitDataCardAdapter(u)).ToList();
        controller.PopulateUnitList(adaptedData, new SelectUnitToPlaceCommand());
        
    }
    public void DisplayEnemyUnitsList()
    {
        InitiativePanel.SetActive(false);
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
            MapTileMapManager.MapTileMapManagerInstance.UpdateGroundTileMap();
        }
    }

    public void DisplayPlayerUnitInfo(BaseUnit selectedUnit)
    {
        InitiativePanel.SetActive(false);
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
        InitiativePanel.SetActive(false);
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
