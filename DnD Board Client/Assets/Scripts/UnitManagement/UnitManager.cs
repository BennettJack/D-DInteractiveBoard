using System;
using System.Collections.Generic;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DataObjects.Units
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager UnitManagerInstance;
        
        private List<IBaseUnit> _units = new();
        private Dictionary<string, IBaseUnit> _unitLookup = new();

        private void Awake()
        {
            UnitManagerInstance = this;
        }

        public void InitUnitList(List<IBaseUnit> units)
        {
            _units = units;
            _unitLookup.Clear();
            foreach (var unit in _units)
            {
                if (!_unitLookup.TryAdd(unit.unitName, unit))
                {
                    Debug.LogError($"Failed to add unit {unit.unitName}");
                }
            }
        }

        public void AddUnit(IBaseUnit unitToAdd)
        {
            foreach (var unit in _units)
            {
                if (!_unitLookup.TryAdd(unit.unitName, unit))
                {
                    Debug.LogError($"Failed to add unit {unit.unitName}");
                }
            }
        }

        public void RemoveUnit(IBaseUnit unit)
        {
            _units.Remove(unit);
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
            Debug.Log("GetAllUnits");
            return new List<IBaseUnit>(_units);
        }
    }
}