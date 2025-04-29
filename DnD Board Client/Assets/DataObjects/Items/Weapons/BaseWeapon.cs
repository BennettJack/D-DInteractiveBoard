using System.Collections.Generic;
using Scriptable_Objects.Units.BaseUnits;

namespace DataObjects.Items.Weapons
{
    public class BaseWeapon : IBaseWeapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WeaponType { get; set; }
        public Dice[] HitDice { get; set; }
        public Dice[] DamageDice { get; set; }
        public int AttackRange { get; set; }
        public int InherentAttackModifier { get; set; }
        public int InherentDamageModifier { get; set; }

        public int AttackRoll(int playerAttackModifier, List<int> rolls)
        {
            return 0;
        }
        
    }
}