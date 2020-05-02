using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Spells/Add Spell")]
public class SpellData : ScriptableObject
{
    public int ID;
    public string Name;
    public Rarity Rarity;
    public List<UnitType> Movement;
    public SpellPattern pattern;
    public Sprite sprite;
    public GameObject anim;
}

/*

public class SpellData
{
    public int ID;
    public string Name;
    public Rarity Rarity;
    public List<UnitType> ableToLearn;
    public int damage;
    public int range;
    public ApplyRange applyRange;
    public bool affectAllies, affectEnemies;
    public SpellEffect effectToApply;
    public float chanceToApply;
    public float critChance;
    public Sprite sprite;
    public GameObject prefab;
}

    */