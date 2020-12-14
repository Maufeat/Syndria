using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation
{
    interface IFormation
    {
        PlayerHero hero { get; set; }
        int x { get; set; }
        int y { get; set; }
    }

    public int width = 2, height = 5;

    public int maxHeroes = 2;

    public PlayerHero[,] heroes;

    public void InitByPacketString(string _packet)
    {
        // {"msgHeader":"UF","formation":[[-1,-1,-1,944,946,-1,-1,-1,-1],[-1,-1,943,-1,-1,-1,-1,-1,-1]]}
        var formationInfo = (JArray)JObject.Parse(_packet)["formation"];
        if (formationInfo == null)
            return;
        
        heroes = new PlayerHero[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Convert.ToInt32(formationInfo[x][y]) != -1)
                {
                    var hero = Client.Instance.me.heroes.Find(hh => hh.id == Convert.ToInt32(formationInfo[x][y]));
                    heroes[x, y] = hero;
                }
                else
                {
                    heroes[x, y] = null;
                }
            }
        }
    }
}
