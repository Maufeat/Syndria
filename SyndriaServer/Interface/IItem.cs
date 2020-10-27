using SyndriaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Interface
{
    public interface IItem
    {
        ItemData itemData { get; set; }
        int qty { get; set; }
        // TODO: SpellData 

        bool OnUse(int qty);
    }
}
