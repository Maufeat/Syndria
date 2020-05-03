using SyndriaServer.Models.FightData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Interface
{
    public interface ISpell
    {
        HeroObject caster { get; set; }
        int cooldown { get; set; }
        Vector2 location { get; set; }
        // TODO: SpellData 

        bool Cast(Vector2 location, AttackableObject target);
    }
}
