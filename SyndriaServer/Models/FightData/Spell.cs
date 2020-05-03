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

        public Spell(HeroObject _caster)
        {
            caster = _caster;
        }

        // Target can be null, because not every spell is targeted
        public virtual bool Cast(Vector2 location, AttackableObject target = null)
        {
            Logger.Write("SyndriaServer.Models.FightData::Cast");
            return true;
        }
    }
}
