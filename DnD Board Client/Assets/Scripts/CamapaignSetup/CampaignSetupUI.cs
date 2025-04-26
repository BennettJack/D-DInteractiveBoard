using System;
using DataObjects.Units;
using UnityEngine;
using UnityEngine.UI;

public class CampaignSetupUI : MonoBehaviour
{
    public static CampaignSetupUI CampaignSetupUiInstance;
    private CampaignSetupController _campaignSetupController;
    
    public ScrollRect playerUnitsScrollRect;
    public ScrollRect friendlyNPCsScrollRect;
    public ScrollRect enemyUnitScrollRect;
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
        Debug.Log("campaign setup ui started");
    }

    public void Test()
    {
        Debug.Log(UnitManager.UnitManagerInstance.GetAllUnits());
    }
}
