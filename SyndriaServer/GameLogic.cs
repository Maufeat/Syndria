using Newtonsoft.Json;
using SyndriaServer.Enums;
using SyndriaServer.Interface;
using SyndriaServer.Models;
using SyndriaServer.Models.FightData;
using SyndriaServer.Scripts.Spells;
using SyndriaServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace SyndriaServer
{
    public class GameLogic
    {
        public static Dictionary<int, ISpell> spellScripts = new Dictionary<int, ISpell>();
        public static Dictionary<int, SpellPattern> spellPatterns = new Dictionary<int, SpellPattern>();
        public static Dictionary<int, SpellData> spells = new Dictionary<int, SpellData>();
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

        public static void LoadSpellScripts()
        {
            spellScripts.Add(1, new Detonation());
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

        public static void LoadSpells()
        {
            LoadSpellScripts(); 
            string path = Directory.GetCurrentDirectory() + "/spells/datas";

            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                dynamic deserialize = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(file));
                
                var id = int.Parse(deserialize["id"].ToString());
                var name = deserialize["name"].ToString();
                var rarity = (Rarity)int.Parse(deserialize["rarity"].ToString());
                int[] canLearn = ConvertStringToIntArray(deserialize["canLearn"].ToString());
                SpellPattern rangePattern = spellPatterns[int.Parse(deserialize["rangePattern"].ToString())];
                SpellPattern attackPattern = spellPatterns[int.Parse(deserialize["attackPattern"].ToString())];
                var spellScript = int.Parse(deserialize["spellScript"].ToString());
                var range = int.Parse(deserialize["range"].ToString());

                SpellData newSpell = new SpellData();

                newSpell.ID = id;
                newSpell.Name = name;
                newSpell.Rarity = rarity;
                newSpell.CanLearn = new List<UnitType>();
                newSpell.rangePattern = rangePattern;
                newSpell.attackPattern = attackPattern;
                newSpell.spellScript = spellScripts[spellScript];
                foreach (var i in canLearn)
                    newSpell.CanLearn.Add((UnitType)i);

                spells.Add(id, newSpell);
            }

            Logger.Write($"Loaded {spells.Count} Spells");
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
