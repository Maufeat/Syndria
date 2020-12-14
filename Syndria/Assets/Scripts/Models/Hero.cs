using System.Collections.Generic;
using UnityEngine;


public class Hero : AttackableObject, IAiObject
{
    public PlayerHero playerHero { get; set; }

    //public List<SpellData> spells { get; }

    public Hero()
    {

    }

    public Hero(PlayerHero hero)
    {
        ID = hero.id;
        playerHero = hero;
    }

    public void OnKill(IAttackableObject killed)
    {
        throw new System.NotImplementedException();
    }
}
