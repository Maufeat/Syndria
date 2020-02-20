using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models
{
    public enum CharType
    {
        Rock,
        Paper,
        Scissor
    }

    public enum Rarity
    {
        R,
        SR,
        SSR,
        UR
    }

    public class CharacterData
    {
        public int ID;
        public string Name;
        public CharType Type;
        public Rarity Rarity;
        //public Stats Stats;
    }

    public class Character : CharacterData
    {
        public string GUID;
        public int Level;
        public int Experience;
        public List<SpellData> Spells;

        public Vector2 location;
    }
}
