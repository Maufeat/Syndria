using UnityEngine;

[CreateAssetMenu(fileName = "hero", menuName = "Add Hero")]
public class HeroData : ScriptableObject
{
    public int Aptitude;
    public int Heatlh;
    public int AttackRange;
    public int Movement;
}
