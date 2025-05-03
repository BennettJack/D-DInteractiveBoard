using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DataObjects.Units
{
    public class UnitLoader : MonoBehaviour
    {
        public static UnitLoader UnitLoaderInstance;
        public List<IBaseUnit> loadedUnits = new();

        private void Awake()
        {
            UnitLoaderInstance = this;
            UnitFactory.RegisterUnitTypes();
        }
        

        public void LoadUnits(string json)
        {
            var unitData = JsonConvert.DeserializeObject<List<JObject>>(json);

            foreach (var unit in unitData)
            {
                if (unit != null)
                {
                    loadedUnits.Add(UnitFactory.CreateUnit(unit));
                }
            }
            
            UnitManager.UnitManagerInstance.AddUnits(loadedUnits);
        }

        public List<IBaseUnit> ConvertUnits(string json)
        {
            var unitData = JsonConvert.DeserializeObject<List<JObject>>(json);
            var unitsToReturn = new List<IBaseUnit>();

            foreach (var unit in unitData)
            {
                if (unit != null)
                {
                    unitsToReturn.Add(UnitFactory.CreateUnit(unit));
                }
            }

            return unitsToReturn;
        }
    }
}