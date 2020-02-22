using UnityEngine;

[CreateAssetMenu(fileName = "hero", menuName = "Add Hero")]
public class HeroData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public UnitType Type;
    public Rarity Rarity;
    public int Aptitude;
    public int Heatlh;
    public int Attack;
    public int AttackRange;
    public int Movement;
}
