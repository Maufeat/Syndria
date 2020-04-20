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
        Client.instance.myId = _myId;
        Client.instance.me = new Player();

        ClientSend.WelcomeReceived();
    }

    public static void UpdateUserData(Packet _packet)
    {
        Client.instance.me.Username = _packet.ReadString();
        Client.instance.me.level = _packet.ReadInt();
        Client.instance.me.exp = _packet.ReadInt();
        Client.instance.me.energy = _packet.ReadInt();
        Client.instance.me.gold = _packet.ReadInt();
        Client.instance.me.diamonds = _packet.ReadInt();
        Client.instance.me.dailyCount = _packet.ReadInt();

        Client.instance.me.heroes = new List<PlayerHero>();
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
            p.baseHeroData = Resources.Load<HeroData>($"Characters/{heroId}/data");
            Client.instance.me.heroes.Add(p);
        }
    }

    public static void CreateCharacter(Packet _packet)
    {
        UIManager.instance.CloseAllPanel();
        UIManager.instance.CloseLoadingBox();
        UIManager.instance.OpenPanel("UICreateCharacter", true);
    }

    public static void GoToTutorial(Packet _packet)
    {
        UIManager.instance.CloseAllPanel();
        UIManager.instance.CloseLoadingBox();
        UIManager.instance.OpenPanel("TutorialFight", true);
    }

    public static void GoToVillage(Packet _packet)
    {
        UIManager.instance.CloseAllPanel(true);
        UIManager.instance.CloseLoadingBox();
        UIManager.instance.OpenPanel("UIVillage", true);
    }

    public static void OpenMessageBox(Packet _packet)
    {
        UIManager.instance.OpenMsgBox(_packet.ReadString());
    }

    public static void ChangeTurn(Packet _packet)
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.EndTurn();
        }
    }
}
 