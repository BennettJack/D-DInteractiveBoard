using System;
using System.IO;
using DataObjects.Campaign;
using Newtonsoft.Json;
using Scriptable_Objects.Units.BaseUnits;
using Scriptable_Objects.Units.BaseUnits.Classes;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestCampaignDataClass
    {
        public void CreateTestCampaign()
        {
            var campaignData = new CampaignData();

            campaignData.Maps = new();
            campaignData.PlayerUnits = new();
            campaignData.EnemyUnits = new();
            campaignData.CampaignName = "Test Campaign";
            var map1 = new MapData()
            {
                MapFileName = "Map1"
            };
            
            var map2 = new MapData()
            {
                MapFileName = "MapTwooooo"
            };


            var tony = new Wizard()
            {
                unitType = "Wizard",
                unitName = "Tony",
            };

            var vlad = new Barbarian()
            {
                unitType = "Barbarian",
                unitName = "Vlad",
            };

            var daisy = new Druid()
            {
                unitType = "Druid",
                unitName = "Daisy",
            };

            var gizmo = new Warlock()
            {
                unitType = "Warlock",
                unitName = "Gizmo",
            };

            var DE1 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "DE1",
            };

            var DE2 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "DE2",
            };

            var DE3 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "DE3",
            };

            var GE1 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "GE1",
            };

            var GE2 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "GE2",
            };

            var GE3 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "GE3",
            };

            var BBEG = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "BBEG",
            };
            
            campaignData.Maps.Add(map1);
            campaignData.Maps.Add(map2);
            campaignData.PlayerUnits.Add(tony);
            campaignData.PlayerUnits.Add(vlad);
            campaignData.PlayerUnits.Add(daisy);
            campaignData.PlayerUnits.Add(gizmo);

            campaignData.EnemyUnits.Add(DE1);
            campaignData.EnemyUnits.Add(DE2);
            campaignData.EnemyUnits.Add(DE3);
            campaignData.EnemyUnits.Add(GE1);
            campaignData.EnemyUnits.Add(GE2);
            campaignData.EnemyUnits.Add(GE3);
            campaignData.EnemyUnits.Add(BBEG);
            
            
            var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client/Campaigns";
            var filePath = Path.Combine(documentsLocation, campaignData.CampaignName + ".json");

            var json = JsonConvert.SerializeObject(
                campaignData,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                });
            Debug.Log(json);

            if (!Directory.Exists(documentsLocation ))
            {
                Directory.CreateDirectory(documentsLocation);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                File.WriteAllText(filePath, json);
            }
            else
            {
                File.WriteAllText(filePath, json);
            }    
        }
    }
}