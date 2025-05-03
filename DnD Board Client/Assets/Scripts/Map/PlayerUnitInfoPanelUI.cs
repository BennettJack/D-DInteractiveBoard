using Scriptable_Objects.Units.BaseUnits;
using TMPro;
using UnityEngine;

namespace Map
{
    public class PlayerUnitInfoPanelUI : MonoBehaviour
    {
        public TMP_Text unitName;
        public TMP_Text unitType;
        public TMP_Text hitPoints;
        public TMP_Text moveSpeed;
        public TMP_Text proficiency;
        public TMP_Text level;
        public TMP_Text strength;
        public TMP_Text dexterity;
        public TMP_Text constitution;
        public TMP_Text intelligence;
        public TMP_Text wisdom;
        public TMP_Text charisma;


        public void UpdatePanel(BaseUnit unit)
        {
            unitName.text = $"Name: {unit.unitName}";
            unitType.text = $"Type: {unit.unitType}";
            hitPoints.text = $"{unit.CurrentHitPoints} / {unit.MaxHitPoints} HP";
            moveSpeed.text = $"Movement Speed: {unit.moveSpeed}";
            proficiency.text = $"Proficiency: {unit.proficiency}";
            level.text = $"Level: {unit.level}";
            strength.text =
                $"Strength: {unit.BaseStrengthAbilityScore} ( + {unit.GetAbilityScoreModifier(unit.BaseStrengthAbilityScore)} )";
            dexterity.text =
                $"Dexterity: {unit.BaseDexterityAbilityScore} ( + {unit.GetAbilityScoreModifier(unit.BaseDexterityAbilityScore)} )";
            constitution.text =
                $"Constitution: {unit.BaseConstitutionAbilityScore} ( + {unit.GetAbilityScoreModifier(unit.BaseConstitutionAbilityScore)} )";
            intelligence.text =
                $"Intelligence: {unit.BaseIntelligenceAbilityScore} ( + {unit.GetAbilityScoreModifier(unit.BaseIntelligenceAbilityScore)} )";
            wisdom.text =
                $"Wisdom: {unit.BaseWisdomAbilityScore} ( + {unit.GetAbilityScoreModifier(unit.BaseWisdomAbilityScore)} )";
            charisma.text =
                $"Charisma: {unit.BaseCharismaAbilityScore} ( + {unit.GetAbilityScoreModifier(unit.BaseCharismaAbilityScore)} )";
        }
    }
}