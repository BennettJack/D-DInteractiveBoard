using System;
using DataObjects.Units;
using UnityEngine;

public class CampaignSetupController : MonoBehaviour
{
    public static CampaignSetupController CampaignSetupControllerInstance;
    private UnitManager _unitManager;
    private CampaignSetupUI _campaignSetupUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        CampaignSetupControllerInstance = this;
    }

    void Start()
    {
        _unitManager = UnitManager.UnitManagerInstance;
        _campaignSetupUI = CampaignSetupUI.CampaignSetupUiInstance;
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
