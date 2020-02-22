using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyndriaServer.Enums;

namespace SyndriaServer.Models
{

    public enum SpellEffect
    {
        Asleep,
        ShortSighted,
        Paralyzed,
        Blind,
        Silenced,
        Charmed,
        Poisoned,
        Burning,
        Frozen
    }

    public enum ApplyRange
    {
        Single,
        Linear,
        AoE,
        All
    }

    public class SpellData
    {
        public int ID;
        public string Name;
        public Rarity Rarity;
        public List<UnitType> ableToLearn;
        public int damage;
        public int range;
        public ApplyRange applyRange;
        public bool affectAllies, affectEnemies;
        public SpellEffect effectToApply;
        public float chanceToApply;
        public float critChance;
    }
}
