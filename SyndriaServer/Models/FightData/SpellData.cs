using SyndriaServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models.FightData
{
    public class SpellData
    {
        public int ID;
        public string Name;
        public Rarity Rarity;
        public List<UnitType> CanLearn;
        public SpellPattern rangePattern;
        public SpellPattern attackPattern;
        public int range;
        public Spell spellScript;
    }
}
