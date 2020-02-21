using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellEffect
{
    Asleep,
    ShortSighted,
    Paralyzed,
    Blind,
    Silenced,
    Charmed,
    Poisoned,
    Burning,
    Frozen
}

public enum ApplyRange
{
    Single,
    Linear,
    AoE,
    All
}

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