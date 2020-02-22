using System.Collections.Generic;
using UnityEngine;


public class Hero : AttackableObject, IAiObject
{
    public HeroData heroData { get; set; }

    public List<SpellData> spells { get; }

    public void OnKill(IAttackableObject killed)
    {
        throw new System.NotImplementedException();
    }
}
