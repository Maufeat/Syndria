using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Hero/Add Hero")]
public class HeroData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public UnitType Type;
    public Rarity BaseRarity;
    public HeroStats BaseStats;
    public GameObject overwriteGameObject;
}
