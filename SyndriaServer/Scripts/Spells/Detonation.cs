using SyndriaServer.Interface;
using SyndriaServer.Models;
using SyndriaServer.Models.FightData;
using SyndriaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Scripts.Spells
{
    public class Detonation : ISpell
    {
        public HeroObject caster { get; set; }
        public int cooldown { get; set; }
        public Vector2 location { get; set; }
        
        public bool Cast(Fight fight, Vector2 _location, List<Tile> aoe)
        {
            location = _location;
            foreach(var tile in aoe)
            {
                if(tile.objectOnTile != null)
                {
                    if(tile.objectOnTile is HeroObject)
                    {
                        HeroObject hero = tile.objectOnTile as HeroObject;
                        hero.TakeDamage(caster, 500, false);
                    }
                }
            }
            Logger.Write("Detonation:ISpell::Cast");
            return true;
        }
    }
}
