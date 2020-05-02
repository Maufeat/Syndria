using SyndriaServer.Interface;
using SyndriaServer.Models.PlayerData;
using System;
using System.Collections.Generic;

namespace SyndriaServer.Models.FightData
{
    public class HeroObject : AttackableObject, IAiObject
    {
        public HeroData baseHero { get; set; }
        public List<SpellData> spellData;
        
        public void OnKill(IAttackableObject killed)
        {
            throw new NotImplementedException();
        }

        public void FromPlayer(PlayerHeroData _pHero)
        {
            ID = _pHero.id;
            baseHero = _pHero.baseHero;
        }
    }
}
