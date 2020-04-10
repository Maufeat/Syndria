using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)C2S.verifyAccessToken))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(NetworkManager.Instance.FBAccessToken.TokenString);

            SendTCPData(_packet);
        }
    }

    public static void SetPrepCharacters(Hero hero)
    {
        using (Packet _packet = new Packet((int)C2S.setPrepCharacter))
        {
            _packet.Write(hero.ID);
            _packet.Write(hero.location.x);
            _packet.Write(hero.location.y);

            Debug.Log($"Set Figure ID {hero.ID} to {hero.location.x}/{hero.location.y}");

            SendTCPData(_packet);
        }
    }

    public static void SendCreateCharacter(int heroId, string nickname)
    {
        using (Packet _packet = new Packet((int)C2S.createCharacter))
        {
            _packet.Write(heroId);
            _packet.Write(nickname);

            SendTCPData(_packet);
        }
    }

    #endregion
}