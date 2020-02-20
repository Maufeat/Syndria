using System;
using System.Collections.Generic;

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

    public List<Character> heroes { get; set; }
    
    /*public void UpdatePlayerData(LittleEndianReader data)
    {
        Username = data.ReadSizedString();
        level = data.ReadInt();
        exp = data.ReadInt();
        gold = data.ReadInt();
        diamonds = data.ReadInt();
    }*/
}
