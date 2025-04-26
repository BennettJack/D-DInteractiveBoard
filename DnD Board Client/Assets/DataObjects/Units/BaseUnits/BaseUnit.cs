using UnityEngine;

namespace Scriptable_Objects.Units.BaseUnits
{
    [System.Serializable]
    public class BaseUnit : IBaseUnit
    {
        public string unitID;
        public string unitType { get; set; }
        public string unitName { get; set; }
        public int moveSpeed { get; private set; }
        
        public int proficiency { get; private set; }
        public int level { get; private set; }
        public int BaseStrengthAbilityScore { get; private set; }
        public int BaseDexterityAbilityScore { get; private set; }
        public int BaseConstitutionAbilityScore { get; private set; }
        public int BaseIntelligenceAbilityScore { get; private set; }
        public int BaseWisdomAbilityScore { get; private set; }
        public int BaseCharismaAbilityScore { get; private set; }


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