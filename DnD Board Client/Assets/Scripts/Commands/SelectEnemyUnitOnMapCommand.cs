using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DefaultNamespace.Commands
{
    public class SelectEnemyUnitOnMapCommand : IUnitCommand
    {
        public void Execute(GameObject unit)
        {
            MapManager.MapManagerInstance.currentlySelectedUnit = unit;
            MapUI.Instance.DisplayEnemyUnitInfo(unit.GetComponent<BaseUnitController>().BaseUnit as BaseUnit);
        }
    }
}