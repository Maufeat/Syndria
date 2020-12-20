using Assets.Scripts.Battle;
using Assets.Scripts.Interface.Battle;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Spells/Add Spell")]
public class SpellData : ScriptableObject
{
    public int ID;
    public string Name;
    public Rarity Rarity;
    public List<UnitType> CanLearn;
    public SpellPattern rangePattern;
    public SpellPattern attackPattern;
    public Sprite sprite;
    public int range;
    public GameObject prefab;
    public GameObject anim;
}