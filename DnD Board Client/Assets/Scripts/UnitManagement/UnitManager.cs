using System;
using System.Collections.Generic;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DataObjects.Units
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager UnitManagerInstance;
        
        private Dictionary<string, IBaseUnit> _unitLookup = new();

        private void Awake()
        {
            UnitManagerInstance = this;
        }

        public void AddUnit(IBaseUnit unitToAdd)
        {
                try
                {
                    _unitLookup.TryAdd(unitToAdd.unitName, unitToAdd);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
        }
        
        public void AddUnits(List<IBaseUnit> units)
        {
            foreach (var unit in units)
            {
                try
                {
                    _unitLookup.TryAdd(unit.unitName, unit);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void RemoveUnit(IBaseUnit unit)
        {
            if (_unitLookup.ContainsKey(unit.unitName))
            {
                _unitLookup.Remove(unit.unitName);
            }
        }

        public IBaseUnit GetUnitByName(string unitName)
        {
            _unitLookup.TryGetValue(unitName, out IBaseUnit unit);
            return unit;
        }

        public List<IBaseUnit> GetAllUnits()
        {
            var units = new List<IBaseUnit>();
            Debug.Log("GetAllUnits");
            foreach (var unit in _unitLookup)
            {
                units.Add(unit.Value);
            }
            return new List<IBaseUnit>(units);
        }
    }
}