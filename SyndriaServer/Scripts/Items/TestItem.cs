using SyndriaServer.Interface;
using SyndriaServer.Models;
using SyndriaServer.Utils;

namespace SyndriaServer.Scripts.Items
{
    class TestItem : IItem
    {
        public ItemData itemData { get; set; }
        public int qty { get; set; }

        public bool OnUse(int qty)
        {
            Logger.Write("Use Item: " + qty);
            return true;
        }
    }
}
