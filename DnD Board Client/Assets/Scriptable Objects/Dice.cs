using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Scriptable Objects/Dice")]
public class Dice : ScriptableObject
{
    public Sprite diceSprite;
    public int sides;
}
