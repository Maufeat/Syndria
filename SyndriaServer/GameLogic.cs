using Newtonsoft.Json;
using SyndriaServer.Models;
using SyndriaServer.Utils;
using System.Collections.Generic;
using System.IO;

namespace SyndriaServer
{
    public class GameLogic
    {
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

        public static void LoadAllMaps()
        {
            string path = Directory.GetCurrentDirectory() + "/maps";

            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                MapData deserialize = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(file));
                Logger.Write($"Loaded Map: {deserialize.name} Maps");
                maps.Add(deserialize.id, deserialize);
            }
        }
    }
}
