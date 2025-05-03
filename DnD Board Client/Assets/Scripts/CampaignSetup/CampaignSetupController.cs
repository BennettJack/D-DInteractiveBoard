using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataObjects.Adapters;
using DataObjects.Units;
using DefaultNamespace;
using DefaultNamespace.CampaignSetup;
using Newtonsoft.Json.Linq;
using Scriptable_Objects.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine;


public class CampaignSetupController : MonoBehaviour
{
    public static CampaignSetupController CampaignSetupControllerInstance;
    private UnitManager _unitManager;
    private CampaignLoader _campaignLoader;
    private List<IBaseUnit> _playerUnits;
    private List<IBaseUnit> _enemyUnits;
    
    private CampaignSetupUI _campaignSetupUI;
    private List<IBaseUnit> _selectedPlayerUnits = new ();

    private List<IBaseUnit> _selectedEnemyUnits = new ();

    private List<string> _selectedMaps = new ();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        CampaignSetupControllerInstance = this;
    }

    void Start()
    {
        _unitManager = UnitManager.UnitManagerInstance;
        _campaignSetupUI = CampaignSetupUI.CampaignSetupUiInstance;
        _campaignLoader = CampaignLoader.Instance;
        LoadUnitsFromFile();

        /*CampaignLoader campLoader = new CampaignLoader();
        var campData = campLoader.LoadCampaignData("Test Campaign.json");
        
       
        
        _playerUnits = UnitLoader.UnitLoaderInstance.ConvertUnits(playerUnits);
        _enemyUnits = UnitLoader.UnitLoaderInstance.ConvertUnits(enemyUnits);*/
        
       _campaignSetupUI.playerUnitsScrollRect.AddComponent<UnitSelectorController>();
       _campaignSetupUI.enemyUnitScrollRect.AddComponent<UnitSelectorController>();
       _campaignSetupUI.availableSelectionsScrollRect.AddComponent<UnitSelectorController>();
       _campaignSetupUI.selectedScrollRect.AddComponent<UnitSelectorController>();
       
       var playerUnitData = _playerUnits.Select(u => (ICardData)new UnitDataCardAdapter(u)).ToList();
       _campaignSetupUI.playerUnitsScrollRect.AddComponent<UnitSelectorController>().PopulateUnitList(playerUnitData);
       
       var enemyUnitData = _enemyUnits.Select(u => (ICardData)new UnitDataCardAdapter(u)).ToList();
       _campaignSetupUI.enemyUnitScrollRect.AddComponent<UnitSelectorController>().PopulateUnitList(enemyUnitData);
       _campaignSetupUI.selectorPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        Debug.Log("Test");
        var test =_unitManager.GetAllUnits();
    }

    public List<IBaseUnit> GetSelectedPlayerUnits()
    {
        return _selectedPlayerUnits;
    }
    
    public List<IBaseUnit> GetSelectedEnemyUnits()
    {
        return _selectedEnemyUnits;
    }

    void LoadUnitsFromFile()
    {
        
        
        var testAvailLoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client";
        var testAvailFP = Path.Combine(testAvailLoc, "availableUnits.json");
        var testAvailJson = File.ReadAllText(testAvailFP);
        
        Debug.Log(testAvailJson);
        UnitLoader.UnitLoaderInstance.LoadUnits(testAvailJson);

    }
}
