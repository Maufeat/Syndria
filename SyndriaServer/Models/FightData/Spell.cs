using SyndriaServer.Interface;
using SyndriaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models.FightData
{
    public class Spell : ISpell
    {
        public HeroObject caster { get; set; }
        public int cooldown { get; set; }
        public Vector2 location { get; set; }
        public SpellData spellData { get; set; }

        public Spell(HeroObject _caster, SpellData _data)
        {
            caster = _caster;
            spellData = _data;
        }

        // Target can be null, because not every spell is targeted
        public virtual bool Cast(Fight fight, Vector2 location, List<Tile> aoe)
        {
            Logger.Write("SyndriaServer.Models.FightData::Cast");
            return true;
        }
    }
}
