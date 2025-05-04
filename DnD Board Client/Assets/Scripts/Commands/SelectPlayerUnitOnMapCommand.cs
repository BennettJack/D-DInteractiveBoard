using System.Windows.Input;
using DefaultNamespace.TurnBasedScripts;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DefaultNamespace.Commands
{
    public class SelectPlayerUnitOnMapCommand : IUnitCommand
    {
        public void Execute(GameObject unitObj)
        {
            Debug.Log("SelectUnitOnMapCommand");
            MapManager.MapManagerInstance.currentlySelectedUnit = unitObj;
            var unit = unitObj.GetComponent<BaseUnitController>().BaseUnit as BaseUnit;
            MapUI.Instance.DisplayPlayerUnitInfo(unit);
            
            Debug.Log(unitObj.name);
            if (TurnBasedModeManager.Instance.GetCurrentTurn() == unitObj.name)
            {
                Debug.Log("It's your turn!");
            }
            else
            {
                Debug.Log("It's no turn!");
            }
        }
    }
}