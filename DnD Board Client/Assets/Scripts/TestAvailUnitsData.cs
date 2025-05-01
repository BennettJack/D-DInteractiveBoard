using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Scriptable_Objects.Units.BaseUnits;
using Scriptable_Objects.Units.BaseUnits.Classes;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestAvailUnitsData
    {
        public void CreateTestAvailUnitsData()
        {
            var unitList = new List<IBaseUnit>();
            
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
            
            var DE4 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "DE4",
            };

            var DE5 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "DE5",
            };

            var DE6 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "DE6",
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
            
            var GE4 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "GE4",
            };
            
            var GE5 = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "GE5",
            };
            
            var GE6= new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "GE6",
            };

            var BBEG = new BaseUnit()
            {
                unitType = "BaseUnit",
                unitName = "BBEG",
            };
            
            unitList.Add(tony);
            unitList.Add(vlad);
            unitList.Add(daisy);
            unitList.Add(gizmo);
            unitList.Add(DE1);
            unitList.Add(DE2);
            unitList.Add(DE3);
            unitList.Add(DE4);
            unitList.Add(DE5);
            unitList.Add(DE6);
            unitList.Add(GE1);
            unitList.Add(GE2);
            unitList.Add(GE3);
            unitList.Add(GE4);
            unitList.Add(GE5);
            unitList.Add(GE6);
            unitList.Add(BBEG);
            
            var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client";
            var filePath = Path.Combine(documentsLocation, "availableUnits.json");

            var json = JsonConvert.SerializeObject(
                unitList,
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