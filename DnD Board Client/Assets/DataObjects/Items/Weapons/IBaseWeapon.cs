using System.Collections.Generic;

namespace DataObjects.Items.Weapons
{
    public interface IBaseWeapon
    {
        string Name { get; set; }
        string Description { get; set; }
        string WeaponType { get; set; }
        int AttackRange { get; set; }
        int InherentAttackModifier { get; set; }
        int InherentDamageModifier { get; set; }
        Dice [] HitDice {get;set;}
        Dice[] DamageDice { get; set; }
        

        int AttackRoll(int playerAttackModifier, List<int> diceRolls);
    }
}