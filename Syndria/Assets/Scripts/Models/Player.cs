using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Player
{
    public string Username { get; set; }
    public DateTime lastLogin { get; set; }
    public DateTime? lastDaily { get; set; }
    public int dailyCount { get; set; }
    public int level { get; set; }
    public int exp { get; set; }
    public int energy { get; set; }
    public DateTime? lastUsedEnergy { get; set; }
    public int gold { get; set; }
    public int diamonds { get; set; }

    public List<PlayerHero> heroes { get; set; }
    public List<ItemData> items { get; set; }

    /*public void UpdatePlayerData(LittleEndianReader data)
    {
        Username = data.ReadSizedString();
        level = data.ReadInt();
        exp = data.ReadInt();
        gold = data.ReadInt();
        diamonds = data.ReadInt();
    }*/

    public void ParseInventoryString(string inventoryString)
    {
        items = new List<ItemData>();
        //split into itemId,Qty
        string[] inventorySplit = inventoryString.Split(';');
        foreach (var itemStack in inventorySplit)
        {
            var itemStackSplit = itemStack.Split(',');

            var itemId = itemStackSplit[0];
            var itemQty = itemStackSplit[1];
            
            var item = Resources.Load<ItemData>($"Items/{itemId}");
            item.qty = Convert.ToInt32(itemQty);

            items.Add(item);
        }

    }

    public string InventoryToString()
    {
        StringBuilder str = new StringBuilder();
        bool isFirst = true;
        foreach (var item in items)
        {
            if (isFirst)
                isFirst = false;
            else
                str.Append(";");

            str.Append($"{item.ID},{item.qty}");
        }
        return str.ToString();
    }
}
