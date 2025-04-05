using UnityEngine;

namespace Scriptable_Objects.Units.BaseUnits
{
    public class BaseUnit : ScriptableObject
    {
        public string unitName { get; private set; }
        public int moveSpeed { get; private set; }


        public void SetMoveSpeed(int uMoveSpeed)
        {
            moveSpeed = uMoveSpeed;
        }

        public void SetUnitName(string uName)
        {
            unitName = uName;
        }
        
    }
}