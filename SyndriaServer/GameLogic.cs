using Newtonsoft.Json;
using SyndriaServer.Models;
using SyndriaServer.Utils;
using System.Collections.Generic;
using System.IO;

namespace SyndriaServer
{
    public class GameLogic
    {
        public static Dictionary<int, HeroData> heroes = new Dictionary<int, HeroData>();
        public static Dictionary<int, MapData> maps = new Dictionary<int, MapData>();
        public static Dictionary<int, Fight> fights = new Dictionary<int, Fight>();

        public static void Update()
        {
            ThreadManager.UpdateMain();

            /*foreach (var fight in fights)
            {
                fight.Value.Update();
            }*/
        }


        public static void LoadHeroBase()
        {
            string path = Directory.GetCurrentDirectory() + "/heroes";

            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                HeroData deserialize = JsonConvert.DeserializeObject<HeroData>(File.ReadAllText(file));
                heroes.Add(deserialize.ID, deserialize);
            }

            Logger.Write($"Loaded {heroes.Count} Heroes");
        }

        public static void LoadAllMaps()
        {
            string path = Directory.GetCurrentDirectory() + "/maps";

            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                MapData deserialize = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(file));
                maps.Add(deserialize.id, deserialize);
            }

            Logger.Write($"Loaded {maps.Count} Maps");
        }
    }
}
