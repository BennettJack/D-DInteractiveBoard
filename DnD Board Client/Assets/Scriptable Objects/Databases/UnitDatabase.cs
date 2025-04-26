using System.Collections.Generic;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace Scriptable_Objects.Databases
{
    [CreateAssetMenu(menuName = "Database/UnitDatabase")]
    public class UnitDatabase : ScriptableObject
    {
        public List<BaseUnit> units;
        
        private Dictionary<string, BaseUnit> _unitDictionary;

        private void Init()
        {
            _unitDictionary = new Dictionary<string, BaseUnit>();

            foreach (var unit in units)
            {
                _unitDictionary[unit.unitID] = unit;
            }
        }

        public BaseUnit GetUnitById(string unitID)
        {
            if (_unitDictionary == null)
            {
                Init();
            }
            
            _unitDictionary.TryGetValue(unitID, out BaseUnit unit);
            return unit;
        }
    }
}