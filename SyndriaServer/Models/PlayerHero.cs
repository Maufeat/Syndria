using System.Collections.Generic;

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
    }
}
