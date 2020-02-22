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

    public static void SetPrepCharacters()
    {
        using (Packet _packet = new Packet((int)C2S.setPrepCharacters))
        {
            _packet.Write(1);
            _packet.Write(BattleManager.instance.battleMap.GetPrepCharacters().Count);
            foreach (Hero figure in BattleManager.instance.battleMap.GetPrepCharacters())
            {
                Debug.Log($"Set Figure ID {figure.ID} to {(int)figure.location.x}/{(int)figure.location.y}");
                _packet.Write(figure.ID);
                _packet.Write((int)figure.location.x);
                _packet.Write((int)figure.location.y);
            }

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