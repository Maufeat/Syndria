using Newtonsoft.Json;
using SyndriaServer.Models;
using SyndriaServer.Models.FightData;
using SyndriaServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace SyndriaServer
{
    public class GameLogic
    {
        public static Dictionary<int, SpellPattern> spellPatterns = new Dictionary<int, SpellPattern>();
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

        public static void LoadSpellPatterns()
        {
            string path = Directory.GetCurrentDirectory() + "/spells/patterns";

            foreach (string file in Directory.EnumerateFiles(path, "*.asset"))
            {
                var content = File.ReadAllText(file);
                var yaml = new YamlStream();
                yaml.Load(new StringReader(content));
                
                var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                var monobehaviour = (YamlMappingNode)mapping.Children[new YamlScalarNode("MonoBehaviour")];

                var id = int.Parse(monobehaviour.Children[new YamlScalarNode("id")].ToString());
                var _width = int.Parse(monobehaviour.Children[new YamlScalarNode("_width")].ToString());
                var _height = int.Parse(monobehaviour.Children[new YamlScalarNode("_height")].ToString());
                var _pattern = ConvertStringToIntArray(monobehaviour.Children[new YamlScalarNode("_pattern")].ToString());

                var pattern = new SpellPattern()
                {
                    id = id,
                    _width = _width,
                    _height = _height,
                    _pattern = _pattern,
                };
                spellPatterns.Add(id, pattern);
            }

            Logger.Write($"Loaded {spellPatterns.Count} Spell Patterns");
        }

        public static int[] ConvertStringToIntArray(string array)
        {
            int[] i = new int[array.ToCharArray().Length];
            for(var c = 0; c < array.ToCharArray().Length; c++)
            {
                i[c] = array.ToCharArray()[c];
            }

            return i;
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
