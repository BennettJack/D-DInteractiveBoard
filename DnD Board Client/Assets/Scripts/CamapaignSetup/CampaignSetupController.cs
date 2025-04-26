using System;
using UnityEngine;

public class CampaignSetupController : MonoBehaviour
{
    public static CampaignSetupController CampaignSetupControllerInstance;
    private CampaignSetupUI _campaignSetupUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        CampaignSetupControllerInstance = this;
    }

    void Start()
    {
        _campaignSetupUI = CampaignSetupUI.CampaignSetupUiInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
