using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models.PlayerData
{
    public class PlayerHeroData
    {
        public int id { get; set; }
        public HeroData baseHero { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public int aptitude { get; set; }
    }
}
