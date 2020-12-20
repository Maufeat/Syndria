using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(string _packet)
    {
        Debug.Log($"Message from server: {_packet}");
        Client.Instance.me = new Player();

        var helloConnectPacket = new HelloConnect();
#if UNITY_EDITOR
        helloConnectPacket.google_id = "103466358977404530792";
#else
        helloConnectPacket.google_id = GooglePlayGames.PlayGamesPlatform.Instance.GetIdToken();
#endif
        helloConnectPacket.version = Client.Instance.v.ToString();
        Client.Instance.Send(helloConnectPacket.Serialize());
    }

    public static void MessageBox(string _packet)
    {
        var msgInfo = JObject.Parse(_packet);
        if (msgInfo["lblId"].ToString() == "TID_LABEL_BETA")
        {
            UIManager.Instance.OpenPanel("TutorialBox");
            return;
        }
        UIManager.Instance.CloseLoadingBox();
        UIManager.Instance.OpenMsgBox(_packet);
    }

    public static void UpdateHeroList(string _packet)
    {
        var heroList = JObject.Parse(_packet)["heroes"];

        Client.Instance.me.heroes = new List<PlayerHero>();
        foreach (var hero in heroList)
        {
            PlayerHero _p = new PlayerHero()
            {
                id = Convert.ToInt32(hero["id"]),
                template = Resources.Load<HeroTemplate>($"Characters/{Convert.ToInt32(hero["template_id"])}/data"),
                owner_id = Convert.ToInt32(hero["owner_id"]),
                level = Convert.ToInt32(hero["level"]),
                xp = Convert.ToInt32(hero["xp"]),
                aptitude = Convert.ToInt32(hero["aptitude"]),
                hat = Convert.ToInt32(hero["hat"]),
                cape = Convert.ToInt32(hero["cape"]),
                amulett = Convert.ToInt32(hero["amulett"]),
                shoes = Convert.ToInt32(hero["shoes"]),
                spellData = new SpellData[4]
                {
                    (Convert.ToInt32(hero["spell1"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{Convert.ToInt32(hero["spell1"])}") : null,
                    (Convert.ToInt32(hero["spell2"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{Convert.ToInt32(hero["spell2"])}") : null,
                    (Convert.ToInt32(hero["spell3"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{Convert.ToInt32(hero["spell3"])}") : null,
                    (Convert.ToInt32(hero["spell4"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{Convert.ToInt32(hero["spell4"])}") : null,
                }
            };
            Client.Instance.me.heroes.Add(_p);
        }
        Debug.LogError("Updated Hero List");
    }

    public static void GameStateLoaded(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var state = Convert.ToInt32(JObject.Parse(_packet)["state"]);
            if (state == 1)
                BattleManager.Instance.AllLoaded();
        }
    }

    public static void SetPrepChar(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var packetData = JObject.Parse(_packet);
            var _id = Convert.ToInt32(packetData["hero_id"]);
            var _location_x = Convert.ToInt32(packetData["x"]);
            var _location_y = Convert.ToInt32(packetData["y"]);
            var _stats = packetData["stats"];

            BattleManager.Instance.GetSetPrepHero(_id, _location_x, _location_y, _stats);
        }
    }

    public static void UpdateUserData(string _packet)
    {
        var dynamicShit = JObject.Parse(_packet)["account"];
        var newPlayer = new Player() {
            id = Convert.ToInt32(dynamicShit["id"]),
            google_id = Convert.ToString(dynamicShit["google_id"]),
            nickname = Convert.ToString(dynamicShit["nickname"]),
            level = Convert.ToInt32(dynamicShit["level"]),
            exp = Convert.ToInt32(dynamicShit["xp"]),
            profile_picture_id = Convert.ToInt32(dynamicShit["profile_picture_id"])
        };
        Client.Instance.me = newPlayer;
        Debug.Log(Client.Instance.me.nickname);
    }

    public static void UpdateHeroFormation(string _packet)
    {
        Debug.Log(_packet);
        Client.Instance.me.currentFormation.InitByPacketString(_packet);
    } 

    public static void UpdateServerConfig(string _packet)
    {
        var configs = JObject.Parse(_packet);
        // xpTable
        JArray tables = (JArray)configs["xpTables"];
        Client.Instance.me.experienceTable = new int[tables.Count + 1];
        Debug.Log(tables.Count);
        int i = 0;
        foreach (var xp in tables)
        {
            Client.Instance.me.experienceTable[i] = Convert.ToInt32(xp);
            Debug.LogError($"Added {Client.Instance.me.experienceTable[i]} exp tables");
            i++;
        }
    }

    public static void UpdateInventory(string _packet)
    {
        var packet = JObject.Parse(_packet);
        Debug.Log(_packet);
        Client.Instance.me.items = new List<ItemData>();
        foreach(var item in packet["items"])
        {
            Debug.Log("Add: " + item);
            var x = Resources.Load<ItemData>($"Items/{item["id"]}");
            if (x != null)
            {
                x.qty = Convert.ToInt32(item["quantity"]);
                Client.Instance.me.items.Add(x);
            }
        }
    }

    public static void CreateCharacter(string _packet)
    {
        UIManager.Instance.CloseAllPanel();
        UIManager.Instance.CloseLoadingBox();

        var configs = JObject.Parse(_packet);
        JArray heroes = (JArray)configs["heroes"];
        Client.Instance.availableCreateCharacter = new int[heroes.Count];

        for (int i = 0; i < heroes.Count; i++)
        {
            Client.Instance.availableCreateCharacter[i] = Convert.ToInt32(heroes[i]);
        }

        UIManager.Instance.OpenPanel("UICreateCharacter", true);
    }

    public static void GoToTutorial(string _packet)
    {
        UIManager.Instance.CloseAllPanel();
        UIManager.Instance.CloseLoadingBox();
        UIManager.Instance.OpenPanel("Battlefield", true);
    }

    public static void GoToVillage(string _packet)
    {
        UIManager.Instance.CloseAllPanel(true);
        UIManager.Instance.CloseLoadingBox();
        UIManager.Instance.OpenPanel("UIVillage", true);
    }

    public static void OpenMessageBox(string _packet)
    {
        ///UIManager.Instance.OpenMsgBox(_packet.ReadString());
    }

    public static void ChangeTurn(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var turnParams = JObject.Parse(_packet);
            BattleManager.Instance.EndTurn(Convert.ToInt32(turnParams["turnTime"]));
        }
    }

    public static void ChangeReadyState(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var ready = Convert.ToBoolean(JObject.Parse(_packet)["ready"]);
            BattleManager.Instance.ChangeReadyState(ready);
        }
    }

    public static void AllLoaded(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.AllLoaded();
        }
    }

    public static void SpawnUnit(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var packet = JObject.Parse(_packet);
            var playerHero = new PlayerHero()
            {
                id = Convert.ToInt32(packet["hero_info"]["id"]),
                owner_id = Convert.ToInt32(packet["hero_info"]["owner_id"]),
                template = Resources.Load<HeroTemplate>($"Characters/{ packet["hero_info"]["template"] }/data"),
                level = Convert.ToInt32(packet["hero_info"]["level"]),
                spellData = new SpellData[4]
                {
                    (Convert.ToInt32(packet["hero_info"]["spells"]["1"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{packet["hero_info"]["spells"]["1"]}") : null,
                    (Convert.ToInt32(packet["hero_info"]["spells"]["2"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{packet["hero_info"]["spells"]["2"]}") : null,
                    (Convert.ToInt32(packet["hero_info"]["spells"]["3"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{packet["hero_info"]["spells"]["3"]}") : null,
                    (Convert.ToInt32(packet["hero_info"]["spells"]["4"]) > 0) ? Resources.Load<SpellData>($"Spells/Data/{packet["hero_info"]["spells"]["4"]}") : null,
                }
            };

            var stats = new Stats()
            {
                health = Convert.ToInt32(packet["hero_info"]["stats"]["health"]),
                attack = Convert.ToInt32(packet["hero_info"]["stats"]["attack"]),
                shield = Convert.ToInt32(packet["hero_info"]["stats"]["shield"]),
                movement = Convert.ToInt32(packet["hero_info"]["stats"]["movement"]),
            };

            var unit = new Hero()
            {
                ID = Convert.ToInt32(packet["hero_info"]["id"]),
                playerHero = playerHero,
                Team = TeamID.RED,
                Stats = stats,
                location = new Vector2Int(Convert.ToInt32(packet["x"]), Convert.ToInt32(packet["y"]))
            };
            Debug.Log(unit.ID + " - " + unit.playerHero.id);
            BattleManager.Instance.SpawnUnit(unit);
        }
    }

    public static void MoveUnit(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var gam = JObject.Parse(_packet);
            BattleManager.Instance.MoveUnit(Convert.ToInt32(gam["hero_id"]), Convert.ToInt32(gam["x"]), Convert.ToInt32(gam["y"]));
        }
    }

    public static void StartFight(string _packet)
    {
        if (BattleManager.Instance == null)
        {
            var configs = JObject.Parse(_packet);
            int mapId = Convert.ToInt32(configs["map_id"]);

            Debug.Log($"RCV Started Fight MapID {mapId}");

            UIManager.Instance.CloseAllPanel(true);
            UIManager.Instance.CloseLoadingBox();
            UIManager.Instance.OpenPanel("Battlefield", true);
        }
    }

    public static void GATEST(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var gaTest = JObject.Parse(_packet);
            var colorTiles = new List<Vector2Int>();
            foreach(JObject x in (JArray)gaTest["cells"])
            {
                colorTiles.Add(new Vector2Int(Convert.ToInt32(x["x"]), Convert.ToInt32(x["y"])));
            }
            BattleManager.Instance.battleMap.WalkingTiles(colorTiles);
        }
    }

    public static void ActionResponse(string _packet)
    {
        Debug.LogError(_packet);

        if (BattleManager.Instance == null)
            return;

        var actionResponse = JObject.Parse(_packet);
        BattleManager.Instance.OnActionResponse(Convert.ToInt32(actionResponse["targetId"]), Convert.ToInt32(actionResponse["action"]), Convert.ToInt32(actionResponse["value"]));
    }

    public static void Attack(string _packet)
    {
        if (BattleManager.Instance != null)
        {
            var attackRequest = JObject.Parse(_packet);
            BattleManager.Instance.Attack(Convert.ToInt32(attackRequest["hero_id"]), Convert.ToInt32(attackRequest["spell_id"]), Convert.ToInt32(attackRequest["x"]), Convert.ToInt32(attackRequest["y"]));
        }
    }

    public static void PleaseUpdate(string _packet)
    {
        UIManager.Instance.OpenPanel("PleaseUpdate");
    }

    public static void EndGameResult(string _packet)
    {
        if(BattleManager.Instance != null)
        {
            BattleManager.Instance.EndGame(JObject.Parse(_packet));
        }
    }
}
