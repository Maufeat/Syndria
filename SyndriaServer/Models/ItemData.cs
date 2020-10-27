using SyndriaServer.Enums;
using SyndriaServer.Interface;

namespace SyndriaServer.Models
{
    public class ItemData
    {
        public int ID;
        public string Name;
        public string Description;
        public ItemType Type;
        public Rarity Rarity;
        public int Quantity;
        public bool IsTwoHanded;
        public int SellPrice;
        public int ScriptId;
        public IItem ItemScript;
    }
}
