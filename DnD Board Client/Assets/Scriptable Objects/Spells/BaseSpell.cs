using UnityEngine;

[CreateAssetMenu(fileName = "BaseSpell", menuName = "Scriptable Objects/Spells/BaseSpell")]
public class BaseSpell : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public string description;
    public int range;
    public AudioClip onCastSound;
    public AudioClip onHitSound;
    public bool hasVerbalComponent;
    public Dice[] diceToRoll;
}
