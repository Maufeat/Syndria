using System.Collections.Generic;
using UnityEngine;

public class PlayerHero
{
    public int id;
    public int owner_id;
    public HeroTemplate template;
    public int level;
    public int xp;
    public int hat;
    public int cape;
    public int amulett;
    public int shoes;
    public SpellData[] spellData = new SpellData[4];
}