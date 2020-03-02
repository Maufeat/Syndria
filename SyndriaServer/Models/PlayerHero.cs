using System.Collections.Generic;
using System.Numerics;

namespace SyndriaServer.Models
{
    public class PlayerHero
    {
        public int ID;
        public int level;
        public int xp;
        public int aptitude;
        public List<SpellData> spellData;
        public HeroData hero;
        public Vector2 location;
    }
}
