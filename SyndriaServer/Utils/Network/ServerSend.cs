using SyndriaServer.Models;
using SyndriaServer.Models.FightData;
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

                _packet.Write(_player.heroes.Count);
                foreach(var hero in _player.heroes)
                {
                    _packet.Write(hero.id);
                    _packet.Write(hero.baseHero.ID);
                    _packet.Write(hero.level);
                    _packet.Write(hero.xp);
                    _packet.Write(hero.aptitude);
                }

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

        public static void EndTurn(List<Client> _toClients)
        {
            using (Packet _packet = new Packet((int)S2C.changeTurn))
            {
                foreach(var id in _toClients)
                    SendTCPData(id.id, _packet);
            }
        }

        public static void ChangeReadyState(Client client, bool _ready)
        {
            using (Packet _packet = new Packet((int)S2C.changeReadyState))
            {
                _packet.Write(_ready);
                SendTCPData(client.id, _packet);
            }
        }

        public static void AllLoaded(List<Client> _toClients)
        {
            using (Packet _packet = new Packet((int)S2C.allLoaded))
            {
                foreach (var id in _toClients)
                    SendTCPData(id.id, _packet);
            }
        }

        public static void SpawnUnit(List<Client> _toClients, HeroObject _heroData)
        {
            using (Packet _packet = new Packet((int)S2C.spawnUnit))
            {

                /*- SpawnUnit

                id
                heroId
                teamId
                location.x
                location.y
                stats
                spells
                */

                Logger.Write($"Spawned Unit as {(int)_heroData.Team}");

                _packet.Write(_heroData.ID);
                _packet.Write(_heroData.baseHero.ID);
                _packet.Write((int)_heroData.Team);
                _packet.Write((int)_heroData.location.X);
                _packet.Write((int)_heroData.location.Y);

                foreach (var id in _toClients)
                    SendTCPData(id.id, _packet);
            }
        }

        public static void MoveHero(List<Client> _toClients, int _id, int _x, int _y)
        {
            using (Packet _packet = new Packet((int)S2C.moveUnit))
            {
                _packet.Write(_id);
                _packet.Write(_x);
                _packet.Write(_y);

                foreach (var id in _toClients)
                    SendTCPData(id.id, _packet);
            }
        }

        public static void Attack(List<Client> _toClients, int _id, int _spellId, int _x, int _y)
        {
            using (Packet _packet = new Packet((int)S2C.attack))
            {
                _packet.Write(_id);
                _packet.Write(_spellId);
                _packet.Write(_x);
                _packet.Write(_y);

                foreach (var id in _toClients)
                    SendTCPData(id.id, _packet);
            }
        }
    }
}
