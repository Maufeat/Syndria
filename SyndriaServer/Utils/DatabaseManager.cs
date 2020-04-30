using System;
using System.Collections.Generic;
using System.Data;

using MySql.Data.MySqlClient;
using SyndriaServer.Enums;
using SyndriaServer.Models;
using SyndriaServer.Models.FightData;
using SyndriaServer.Models.PlayerData;

namespace SyndriaServer.Utils
{
    public static class DatabaseManager
    {
        public static MySqlConnection connection { get; set; }
        public static MySqlDataReader rdr = null;
        public static object Locker = new object();

        public static bool InitConnection()
        {
            try
            {
                Logger.Write("Connecting to Database...");
                connection = new MySqlConnection($"Database={Constants.MYSQL_DB};Data Source={Constants.MYSQL_HOST};User Id={Constants.MYSQL_USER};Password={Constants.MYSQL_PASS};SslMode=none;Convert Zero Datetime=True");
                connection.Open();
                Logger.Write("Connection to Database established");
                return true;
            }
            catch (Exception e)
            {
                Logger.Write("Couldn't connect to database : " + e.Message, LOG_LEVEL.ERROR);
                return false;
            }
        }

        public static Dictionary<string, string> getAccountData(string user, string pass)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM accounts WHERE username='" + user + "' AND password='" + pass + "'";
            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable dtCustomers = new DataTable();
            dtCustomers.Load(reader);
            var dataArray = new Dictionary<string, string>();
            foreach (DataRow row in dtCustomers.Rows)
            {
                dataArray["id"] = row["id"].ToString();
                dataArray["summonerId"] = row["summonerId"].ToString();
                dataArray["RP"] = row["RP"].ToString();
                dataArray["IP"] = row["IP"].ToString();
                dataArray["banned"] = row["isBanned"].ToString();
            }
            return dataArray;
        }

        public static Dictionary<string, string> getSummonerData(string sumId)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM summoner WHERE id='" + sumId + "'";
            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable dtCustomers = new DataTable();
            dtCustomers.Load(reader);
            var dataArray = new Dictionary<string, string>();
            foreach (DataRow row in dtCustomers.Rows)
            {
                dataArray["id"] = row["id"].ToString();
                dataArray["summonerName"] = row["summonerName"].ToString();
                dataArray["icon"] = row["icon"].ToString();
            }
            return dataArray;
        }

        public class DBChampions
        {
            public int ID { get; set; }
            public bool IsFreeToPlay { get; set; }
        }

        public static List<DBChampions> getAllChampions()
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM champions";
            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable dtChampions = new DataTable();
            dtChampions.Load(reader);
            var dataArray = new List<DBChampions>();
            foreach (DataRow row in dtChampions.Rows)
            {
                dataArray.Add(new DBChampions() { ID = Convert.ToInt32(row["id"].ToString()), IsFreeToPlay = Convert.ToBoolean(Convert.ToInt32(row["freeToPlay"].ToString())) });
            }
            return dataArray;
        }

        public static List<int> getAllChampionSkinsForId(int id)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM championSkins WHERE championId='" + id + "'";
            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable dtSkins = new DataTable();
            dtSkins.Load(reader);
            var dataArray = new List<int>();
            foreach (DataRow row in dtSkins.Rows)
            {
                dataArray.Add(Convert.ToInt32(row["id"].ToString()));
            }
            return dataArray;
        }
        
        public static bool createUser(string name, Player p)
        {
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO users (nickname, fb_id, level, energy, gold, diamonds, last_used_energy, last_login, create_time) VALUES (?nickname, ?fb_id, ?level, ?energy, ?gold, ?diamonds, ?last_used_energy, ?last_login, ?create_time)";
                cmd.Parameters.AddWithValue("?nickname", name);
                cmd.Parameters.AddWithValue("?fb_id", p.fbToken.UserId);
                cmd.Parameters.AddWithValue("?level", Constants.START_LEVEL);
                cmd.Parameters.AddWithValue("?energy", Constants.START_ENERGY);
                cmd.Parameters.AddWithValue("?gold", Constants.START_GOLD);
                cmd.Parameters.AddWithValue("?diamonds", Constants.START_ENERGY);
                cmd.Parameters.AddWithValue("?last_used_energy", DateTime.Now);
                cmd.Parameters.AddWithValue("?last_login", DateTime.Now);
                cmd.Parameters.AddWithValue("?create_time", DateTime.Now);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Logger.Write("DBManager createUser failed : " + e.Message, LOG_LEVEL.ERROR);
                return false;
            }
        }

        public static bool addHeroToPlayer(int id, Player p)
        {
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO user_hero (user_id, hero_id, level, exp) VALUES (?user_id, ?hero_id, ?level, ?exp)";
                cmd.Parameters.AddWithValue("?user_id", p.Id);
                cmd.Parameters.AddWithValue("?hero_id", id);
                cmd.Parameters.AddWithValue("?level", 1);
                cmd.Parameters.AddWithValue("?exp", 0);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Logger.Write("DBManager addNinjaToPlayer failed : " + e.Message, LOG_LEVEL.ERROR);
                return false;
            }
        }
        
        public static bool getHeroes(Player p)
        {
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM user_hero WHERE user_id ='" + p.Id + "'";
                MySqlDataReader reader = cmd.ExecuteReader();
                DataTable dtUser = new DataTable();
                dtUser.Load(reader);
                p.heroes = new List<PlayerHeroData>();
                if (dtUser.Rows.Count > 0)
                {
                    foreach (DataRow row in dtUser.Rows)
                    {
                        PlayerHeroData hero = new PlayerHeroData();
                        hero.id = row.Field<int>("id");
                        hero.baseHero = GameLogic.heroes[row.Field<int>("hero_id")];
                        hero.level = row.Field<int>("level");
                        hero.xp = row.Field<int>("exp");
                        p.heroes.Add(hero);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Write("DBManager getAccountByFacebookId failed : " + e.Message, LOG_LEVEL.ERROR);
                return false;
            }
        }
        
        public static bool getAccountByFacebookId(Player p)
        {
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE fb_id ='" + p.fbToken.UserId + "'";
                MySqlDataReader reader = cmd.ExecuteReader();
                DataTable dtUser = new DataTable();
                dtUser.Load(reader);
                if (dtUser.Rows.Count > 0)
                {
                    foreach (DataRow row in dtUser.Rows)
                    {
                        p.Id = row.Field<int>("id");
                        p.Username = row.Field<string>("nickname");
                        p.lastLogin = row.Field<DateTime>("last_login");
                        p.lastDaily = row.Field<DateTime>("last_daily");
                        p.tutorialDone = row.Field<int>("tutorial_done");
                        p.dailyCount = row.Field<int>("daily_count");
                        p.level = row.Field<int>("level");
                        p.exp = row.Field<int>("exp");
                        p.energy = row.Field<int>("energy");
                        p.lastUsedEnergy = row.Field<DateTime>("last_used_energy");
                        p.gold = row.Field<int>("gold");
                        p.diamonds = row.Field<int>("diamonds");
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Write("DBManager getAccountByFacebookId failed : " + e.Message, LOG_LEVEL.ERROR);
                return false;
            }
        }

        public static bool checkAccount(string user, string pass)
        {
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT count(*) FROM accounts WHERE username='" + user + "' AND password='" + pass + "'";
                int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (userCount > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Logger.Write("DBManager checkAccount failed : " + e.Message, LOG_LEVEL.ERROR);
                return false;
            }
        }
    }
}