using System;
using System.Collections.Generic;
using DataObjects.Items.Weapons;
using UnityEngine;

namespace Scriptable_Objects.Units.BaseUnits
{
    [System.Serializable]
    public class BaseUnit : IBaseUnit
    {
        public string unitID;
        public string unitType { get; set; }
        public string unitName { get; set; }
        public int moveSpeed { get;  set; }
        public List<string> weapons { get;  set; }
        
        public int proficiency { get;  set; }
        public int level { get;  set; }
        public int BaseStrengthAbilityScore { get;  set; }
        public int BaseDexterityAbilityScore { get;  set; }
        public int BaseConstitutionAbilityScore { get;  set; }
        public int BaseIntelligenceAbilityScore { get;  set; }
        public int BaseWisdomAbilityScore { get;  set; }
        public int BaseCharismaAbilityScore { get;  set; }
        public int CurrentHitPoints { get;  set; }
        public int MaxHitPoints { get;  set; }


        public void SetMoveSpeed(int uMoveSpeed)
        {
            moveSpeed = uMoveSpeed;
        }

        public void SetUnitName(string uName)
        {
            unitName = uName;
        }

        public int GetAbilityScoreModifier(int baseAbilityScore)
        {
            return  (baseAbilityScore - 10) / 2;
        }
        
    }
}