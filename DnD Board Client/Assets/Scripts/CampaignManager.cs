using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Units;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CampaignManager : MonoBehaviour
    {
        public static CampaignManager Instance;

        public List<string> playerUnitNames;
        public List<string> enemyUnitNames;
        public List<string> mapFileNames = new();

        private void Awake()
        {
            Instance = this;
        }

        public void InitCampaignData(string campaignName, string maps, string playerUnits, string enemyUnits)
        {
            UnitLoader.UnitLoaderInstance.LoadUnits(playerUnits);
            GeneratePlayerNamesList(playerUnits);
            UnitLoader.UnitLoaderInstance.LoadUnits(enemyUnits);
            GenerateEnemyNamesList(enemyUnits);
            GenerateMapList(maps);
        }


        private void GeneratePlayerNamesList(string units)
        {
            var unitObjects = UnitLoader.UnitLoaderInstance.ConvertUnits(units);
            playerUnitNames = unitObjects.Select(u => u.unitName).ToList();
        }
        
        private void GenerateEnemyNamesList(string units)
        {
            var unitObjects = UnitLoader.UnitLoaderInstance.ConvertUnits(units);
            enemyUnitNames = unitObjects.Select(u => u.unitName).ToList();
        }

        private void GenerateMapList(string maps)
        {
            mapFileNames = JsonConvert.DeserializeObject<List<string>>(maps);
            foreach (var map in mapFileNames)
            {
                Debug.Log("Map Name: " + map);
            }

        }
    }
}