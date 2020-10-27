using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.Instance.myId = _myId;
        Client.Instance.me = new Player();

        ClientSend.WelcomeReceived();
    }

    public static void UpdateUserData(Packet _packet)
    {
        Client.Instance.me.Username = _packet.ReadString();
        Client.Instance.me.level = _packet.ReadInt();
        Client.Instance.me.exp = _packet.ReadInt();
        Client.Instance.me.energy = _packet.ReadInt();
        Client.Instance.me.gold = _packet.ReadInt();
        Client.Instance.me.diamonds = _packet.ReadInt();
        Client.Instance.me.dailyCount = _packet.ReadInt();
        Client.Instance.me.ParseInventoryString(_packet.ReadString());

        Client.Instance.me.heroes = new List<PlayerHero>();
        var heroCount = _packet.ReadInt();
        for (int i = 0; i < heroCount; i++)
        {
            var phId = _packet.ReadInt();
            var heroId = _packet.ReadInt();
            PlayerHero p = new PlayerHero()
            {
                ID = phId,
                level = _packet.ReadInt(),
                xp = _packet.ReadInt(),
                aptitude = _packet.ReadInt()
                
            };
            //Give player a Skill.
            p.spellData = new List<SpellData>();
            p.spellData.Add(Resources.Load<SpellData>("Spells/Data/1"));
            p.baseHeroData = Resources.Load<HeroData>($"Characters/{heroId}/data");
            Client.Instance.me.heroes.Add(p);
        }
    }

    public static void CreateCharacter(Packet _packet)
    {
        UIManager.Instance.CloseAllPanel();
        UIManager.Instance.CloseLoadingBox();
        UIManager.Instance.OpenPanel("UICreateCharacter", true);
    }

    public static void GoToTutorial(Packet _packet)
    {
        UIManager.Instance.CloseAllPanel();
        UIManager.Instance.CloseLoadingBox();
        UIManager.Instance.OpenPanel("Battlefield", true);
    }

    public static void GoToVillage(Packet _packet)
    {
        UIManager.Instance.CloseAllPanel(true);
        UIManager.Instance.CloseLoadingBox();
        UIManager.Instance.OpenPanel("UIVillage", true);
    }

    public static void OpenMessageBox(Packet _packet)
    {
        UIManager.Instance.OpenMsgBox(_packet.ReadString());
    }

    public static void ChangeTurn(Packet _packet)
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.EndTurn();
        }
    }

    public static void ChangeReadyState(Packet _packet)
    {
        if (BattleManager.Instance != null)
        {
            var ready = _packet.ReadBool();
            BattleManager.Instance.ChangeReadyState(ready);
        }
    }

    public static void AllLoaded(Packet _packet)
    {
        if(BattleManager.Instance != null)
        {
            BattleManager.Instance.AllLoaded();
        }
    }


    public static void SpawnUnit(Packet _packet)
    {
        if (BattleManager.Instance != null)
        {
            int _id = _packet.ReadInt();
            int _heroId = _packet.ReadInt();
            TeamID _teamId = (TeamID)_packet.ReadInt();
            int _location_x = _packet.ReadInt();
            int _location_y = _packet.ReadInt();

            var unit = new Hero()
            {
                ID = _id,
                heroData = Resources.Load<HeroData>($"Characters/{ _heroId }/data"),
                Team = _teamId,
                location = new Vector2(_location_x, _location_y),
            };
            
            BattleManager.Instance.SpawnUnit(unit);
        }
    }

    public static void MoveUnit(Packet _packet)
    {
        if (BattleManager.Instance != null)
        {
            int _id = _packet.ReadInt();
            int _location_x = _packet.ReadInt();
            int _location_y = _packet.ReadInt();

            BattleManager.Instance.MoveUnit(_id, _location_x, _location_y);
        }
    }

    public static void StartFight(Packet _packet)
    {
        if(BattleManager.Instance == null)
        {
            int mapId = _packet.ReadInt();

            Debug.Log($"RCV Started Fight MapID {mapId}");

            UIManager.Instance.CloseAllPanel(true);
            UIManager.Instance.CloseLoadingBox();
            UIManager.Instance.OpenPanel("Battlefield", true);
        }
    }

    public static void Attack(Packet _packet)
    {
        /*
                _packet.Write(_id);
                _packet.Write(_spellId);
                _packet.Write(_x);
                _packet.Write(_y);
        */

        if (BattleManager.Instance != null)
        {
            int _id = _packet.ReadInt();
            int _spellId = _packet.ReadInt();
            int _location_x = _packet.ReadInt();
            int _location_y = _packet.ReadInt();

            BattleManager.Instance.Attack(_id, _spellId, _location_x, _location_y);
        }
    }
}
