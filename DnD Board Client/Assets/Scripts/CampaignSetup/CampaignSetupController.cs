using System;
using System.Collections.Generic;
using DataObjects.Units;
using DefaultNamespace;
using Scriptable_Objects.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine;

public class CampaignSetupController : MonoBehaviour
{
    public static CampaignSetupController CampaignSetupControllerInstance;
    private UnitManager _unitManager;
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

       _campaignSetupUI.playerUnitsScrollRect.AddComponent<UnitSelectorController>();
       _campaignSetupUI.enemyUnitScrollRect.AddComponent<UnitSelectorController>();
       _campaignSetupUI.availableSelectionsScrollRect.AddComponent<UnitSelectorController>();
       _campaignSetupUI.selectedScrollRect.AddComponent<UnitSelectorController>();
       
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
}
