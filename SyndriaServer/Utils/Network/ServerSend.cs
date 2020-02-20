using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Utils.Network
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)S2C.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void UserData(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)S2C.userData))
            {
                _packet.Write(_player.Username);
                _packet.Write(_player.level);
                _packet.Write(_player.exp);
                _packet.Write(_player.energy);
                _packet.Write(_player.gold);
                _packet.Write(_player.diamonds);
                _packet.Write(_player.dailyCount);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void CreateCharacter(int _toClient)
        {
            using (Packet _packet = new Packet((int)S2C.createCharacter))
            {
                SendTCPData(_toClient, _packet);
            }
        }

        public static void GoToVillage(int _toClient)
        {
            using (Packet _packet = new Packet((int)S2C.toVillage))
            {
                SendTCPData(_toClient, _packet);
            }
        }

        public static void GoToTutorial(int _toClient, int _id)
        {
            using (Packet _packet = new Packet((int)S2C.toTutorial))
            {
                _packet.Write(_id);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void OpenMessageBox(int _toClient, string msg)
        {
            using (Packet _packet = new Packet((int)S2C.messageBox))
            {
                _packet.Write(msg);
                SendTCPData(_toClient, _packet);
            }
        }
    }
}
