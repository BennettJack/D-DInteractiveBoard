using DataObjects.Units;
using Scriptable_Objects.Units.BaseUnits;

namespace DefaultNamespace.TurnBasedScripts
{
    public class UnitTurn
    {
        public int RemainingMovementSpeed;
        
        public UnitTurn(string unitName)
        {
            var unit = UnitManager.UnitManagerInstance.GetUnitByName(unitName) as BaseUnit;
            if (unit != null)
            {
                //TODO - Change this to use the mapdata movement cost per tile
                RemainingMovementSpeed = unit.moveSpeed / 5;    
            }
        }

        public void UpdateRemainingMovementSpeed()
        {
            RemainingMovementSpeed -= 1;
        }
    }
}