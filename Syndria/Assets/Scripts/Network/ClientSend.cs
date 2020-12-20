using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendData(string _packet)
    {
        Client.Instance.Send(_packet);
    }

    #region Packets

    public static void SetPrepCharacters(Hero hero)
    {
        SendData($"GSP|{hero.ID},{hero.location.x},{hero.location.y}");
        Debug.Log($"Set Figure ID {hero.ID} to {hero.location.x}/{hero.location.y}");
    }

    public static void UseItem(int id, int qty)
    {
        JObject useItem = new JObject();
        useItem["msgHeader"] = "UseItem";
        useItem["id"] = id;
        useItem["qty"] = qty;
        SendData(JsonConvert.SerializeObject(useItem));
    }

    public static void DeleteFormation()
    {
        JObject formation = new JObject();
        formation["msgHeader"] = "SRF";
        SendData(JsonConvert.SerializeObject(formation));
    }

    public static void SetFormation(PlayerHero hero, Vector2 location)
    {
        JObject formation = new JObject();
        formation["msgHeader"] = "SF";
        formation["id"] = hero.id;
        formation["x"] = location.x;
        formation["y"] = location.y;
        SendData(JsonConvert.SerializeObject(formation));
    }

    public static void SetPrepCharacters(PlayerHero hero, Vector2 location)
    {
        JObject gspPacket = new JObject();
        gspPacket["msgHeader"] = "GSP";
        gspPacket["owner_id"] = hero.owner_id;
        gspPacket["hero_id"] = hero.id;
        gspPacket["x"] = location.x;
        gspPacket["y"] = location.y;
        SendData(JsonConvert.SerializeObject(gspPacket));
        Debug.Log($"Set Figure ID {hero.id} to {location.x}/{location.y}");
    }

    public static void StartFight(int mapId)
    {
        JObject startFight = new JObject();
        startFight["msgHeader"] = "FS";
        startFight["type"] = "TEST";
        startFight["map_id"] = mapId;
        SendData(JsonConvert.SerializeObject(startFight));
    }

    public static void Test()
    {
        JObject test = new JObject();
        test["msgHeader"] = "TEST";
        SendData(JsonConvert.SerializeObject(test));
    }

    public static void SendCreateCharacter(int heroId, string nickname)
    {
        JObject cc = new JObject();
        cc["msgHeader"] = "CC";
        cc["nickname"] = nickname;
        cc["start_hero"] = heroId;
        SendData(JsonConvert.SerializeObject(cc));
    }

    public static void ClientLoaded()
    {
        JObject cl = new JObject();
        cl["msgHeader"] = "GCL";
        SendData(JsonConvert.SerializeObject(cl));
    }

    public static void EndTurn()
    {
        JObject get = new JObject();
        get["msgHeader"] = "GET";
        SendData(JsonConvert.SerializeObject(get));
    }

    public static void SendPacket(string msg)
    {
        JObject dynamicMsg = new JObject();
        dynamicMsg["msgHeader"] = msg;
        SendData(JsonConvert.SerializeObject(dynamicMsg));
    }

    public static void MoveUnit(int unitId, int x, int y)
    {
        JObject gam = new JObject();
        gam["msgHeader"] = "GAM";
        gam["hero_id"] = unitId;
        gam["x"] = x;
        gam["y"] = y;
        SendData(JsonConvert.SerializeObject(gam));
    }

    public static void Attack(int unitId, int spellId, int x, int y)
    {
        JObject gas = new JObject();
        gas["msgHeader"] = "GAS";
        gas["hero_id"] = unitId;
        gas["spell_id"] = spellId;
        gas["x"] = x;
        gas["y"] = y;
        SendData(JsonConvert.SerializeObject(gas));
    }

    public static void ChangeReadyState(bool ready)
    {
        JObject grs = new JObject();
        grs["msgHeader"] = "GSR";
        grs["ready"] = ready;
        SendData(JsonConvert.SerializeObject(grs));
    }

#endregion
}