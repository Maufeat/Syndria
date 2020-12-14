using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Hero/Add Hero")]
public class HeroTemplate : ScriptableObject
{
    public int id;
    public new string name;
    public UnitType type;
    public Rarity rarity;
    public int health;
    public int strength;
    public int inteligence;
    public int luck;
    public int agility;
    public int startSpellId;
    public int movement;
    public string description;
    public GameObject overwriteGameObject;
}
