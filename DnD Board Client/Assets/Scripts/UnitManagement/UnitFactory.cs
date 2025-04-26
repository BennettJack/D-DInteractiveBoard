using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Scriptable_Objects.Units.BaseUnits;
using Scriptable_Objects.Units.BaseUnits.Classes;
using UnityEngine;

namespace DataObjects.Units
{
    //A class to create units
    public class UnitFactory
    {
        private static readonly Dictionary<string, Type> unitTypes = new();

        public static void RegisterUnitTypes()
        {
            var unitType = typeof(IBaseUnit);
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => unitType.IsAssignableFrom(type));

            foreach (var type in types)
            {
                unitTypes[type.Name] = type;
            }
        }
        
        public static IBaseUnit CreateUnit(JObject unitJson)
        {
            string unitTypeName = unitJson["unitType"]?.ToString();
            if (string.IsNullOrEmpty(unitTypeName))
            {
                Debug.LogWarning("data does not have unit type defined");
                return null;
            }

            if (unitTypes.TryGetValue(unitTypeName, out Type unitType))
            {
                return (IBaseUnit)unitJson.ToObject(unitType);
            }

            else
            {
                Debug.LogWarning($"unit type {unitTypeName} is not registered");
                return null;
            }
        }
    }
}