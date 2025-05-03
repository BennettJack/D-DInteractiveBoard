using System;
using System.IO;
using DataObjects.Units;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CampaignLoader : MonoBehaviour
    {
        public static CampaignLoader Instance;
        private void Awake()
        {
            Instance = this;
        }

        public void LoadCampaignData(string filename)
        {
            var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client/Campaigns";

            var filePath = Path.Combine(documentsLocation, filename);
            var json = File.ReadAllText(filePath);
        
            var campData = JObject.Parse(json);
            
            var campaignName = campData["CampaignName"]?.ToString();
            var maps = campData["Maps"]?.ToString();
            var playerUnits = campData["PlayerUnits"]?.ToString();
            var enemyUnits = campData["EnemyUnits"]?.ToString();
            
            CampaignManager.Instance.InitCampaignData(campaignName, maps, playerUnits, enemyUnits);

        }
    }
}