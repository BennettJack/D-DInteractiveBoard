using System.Windows.Input;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DefaultNamespace.Commands
{
    public class SelectUnitOnMapCommand : IUnitCommand
    {
        public void Execute(GameObject unit)
        {
            Debug.Log("SelectUnitOnMapCommand");
            MapManager.MapManagerInstance.currentlySelectedUnit = unit;
            MapUI.Instance.DisplayPlayerUnitInfo(unit.GetComponent<BaseUnitController>().BaseUnit as BaseUnit);
        }
    }
}