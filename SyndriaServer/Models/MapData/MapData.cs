using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models
{
    public class MapData
    {
        public int id;
        public string name;
        public string mode;
        public string difficulty;
        public List<MobData> mobs;

        // Appearance
        public string background = "map_grass_summer_day";
        public string music = "";
    }

    public class MobData
    {
        public int id;
        public int x;
        public int y;
        public int baseHeroId;
        public MobStatsData stats;
        public List<int> spells;
    }

    public class MobStatsData
    {
        public int HP;
    }
}
