using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Utils
{
    public static class Constants
    {
        public const int SERVER_PORT = 1337;

        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;

        // START ACCOUNT CONFIGS
        public const int START_LEVEL = 1;
        public const int START_ENERGY = 0;
        public const int START_GOLD = 0;
        public const int START_DIAMONDS = 0;

        // VILLAGE CONFIGS
        public static List<int> VILLAGE_WIDTH_BY_LEVEL = new List<int>(){
            1, // 1
            1, // 2
            1, // 3
            1, // 4
            1, // 5
            1, // 6
            1, // 7
            1, // 8
            1, // 9
            1, // 10
            5, // 11
        };

        public const int MAX_VILLAGE_WIDTH = 20;

        public const string MYSQL_HOST = "127.0.0.1";
        public const string MYSQL_USER = "root";
        public const string MYSQL_PASS = "123123";
        public const string MYSQL_DB = "game";
    }
}
