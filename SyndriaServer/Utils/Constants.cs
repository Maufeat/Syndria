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

        public const int START_LEVEL = 1;
        public const int START_ENERGY = 0;
        public const int START_GOLD = 0;
        public const int START_DIAMONDS = 0;
        
        public const string MYSQL_HOST = "127.0.0.1";
        public const string MYSQL_USER = "root";
        public const string MYSQL_PASS = "123123";
        public const string MYSQL_DB = "game";
    }
}
