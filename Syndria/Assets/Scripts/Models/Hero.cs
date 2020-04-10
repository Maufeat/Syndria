using System.Collections.Generic;
using UnityEngine;


public class Hero : AttackableObject, IAiObject
{
    public HeroData heroData { get; set; }
    public PlayerHero playerHero { get; set; }

    public List<SpellData> spells { get; }

    public Hero()
    {

    }

    public Hero(PlayerHero hero)
    {
        ID = hero.ID;
        playerHero = hero;
        heroData = playerHero.baseHeroData;
    }

    public void OnKill(IAttackableObject killed)
    {
        throw new System.NotImplementedException();
    }
}
