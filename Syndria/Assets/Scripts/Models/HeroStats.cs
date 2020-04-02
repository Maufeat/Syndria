using UnityEngine;

[CreateAssetMenu(fileName = "stats", menuName = "Hero/Add Stats")]
public class HeroStats : ScriptableObject
{
    public int Attack;
    public int Defense;
    public int HP;
    public int PierceRate;
    public int Resistance;
    public int Regeneration;
    public int CritChance;
    public int CritDamage;
    public int CritResistance;
    public int CritDefense;
    public int RecoveryRate;
    public int Lifesteal;
}
