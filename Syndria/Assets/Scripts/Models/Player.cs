using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Player
{
    public int id;
    public string nickname;
    public string google_id;
    public string last_login;
    public int profile_picture_id;
    public int tutorial_step;
    public string last_daily;
    public int daily_count;
    public int level;
    public int exp;
    public int energy;
    public string last_used_energy;
    public int gold;
    public int diamonds;

    public int[] experienceTable = { }; 

    public List<PlayerHero> heroes { get; set; }
    public List<ItemData> items { get; set; }

    public Formation currentFormation = new Formation();

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
