using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Adapters;
using DataObjects.Units;
using DefaultNamespace;
using DefaultNamespace.CampaignSetup;
using DefaultNamespace.Commands;
using UnityEngine;
using UnityEngine.UI;

public class CampaignSetupUI : MonoBehaviour
{
    public static CampaignSetupUI CampaignSetupUiInstance;
    private CampaignSetupController _campaignSetupController;
    
    public ScrollRect playerUnitsScrollRect;
    public ScrollRect friendlyNPCsScrollRect;
    public ScrollRect enemyUnitScrollRect;
    
    public ScrollRect availableSelectionsScrollRect;
    public ScrollRect selectedScrollRect;

    public GameObject selectorPanel;
    private Dictionary<string, GameObject> _selectedPlayerUnitCards = new();
    private Dictionary<string, GameObject> _selectedMapUnitCards = new();
    private Dictionary<string, GameObject> _selectedEnemyUnitCards = new();
    private List<GameObject> _tempActiveCards = new();
    private void Awake()
    {
        CampaignSetupUiInstance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _campaignSetupController = CampaignSetupController.CampaignSetupControllerInstance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Test()
    {
        _campaignSetupController.Test();
    }

    public void OnMouseDown()
    {
        Debug.Log("help");
    }

    public void GenerateSelectorPopup(string contentType)
    {
        switch (contentType)
        {
            case "playerUnits":
                GeneratePlayerUnitPopup();
                break;
            case "enemyUnits":
                GenerateEnemyUnitPopup();
                break;
            case "maps":
                GenerateMapPopup();
                break;
        }
    }

    private void GeneratePlayerUnitPopup()
    {
        var units = UnitManager.UnitManagerInstance.GetAllUnits();
        var adaptedData = units.Select(u => (ICardData)new UnitDataCardAdapter(u)).ToList();
        selectorPanel.SetActive(true);
        availableSelectionsScrollRect.GetComponent<UnitSelectorController>().PopulateUnitList(adaptedData);
    }
    private void GenerateEnemyUnitPopup()
    {
        Debug.Log("EnemyUnitPopup");
    }
    private void GenerateMapPopup()
    {
        Debug.Log("MapPopup");
    }

    public void UpdatePlayerUnitsScrollRect()
    {
        var playerUnits = _campaignSetupController.GetSelectedPlayerUnits();

    }
}
