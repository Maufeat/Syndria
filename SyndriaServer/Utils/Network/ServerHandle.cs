using SyndriaServer.Models;
using SyndriaServer.Models.FightData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Utils.Network
{
    class ServerHandle
    {
        public static void VerifyAccessToken(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _accessToken = _packet.ReadString();
            
            var facebookClient = new FacebookClient();
            var facebookService = new FacebookService(facebookClient);
            var verifyTask = facebookService.VerifyAccessToken(_accessToken);
            Task.WaitAll(verifyTask);
            var fbToken = verifyTask.Result;

            Logger.Write($"[>>][{_fromClient}] Token is {fbToken.IsValid} - Logging in.");

            var player = new Player();
            player.fbToken = fbToken;

            if (DatabaseManager.getAccountByFacebookId(player))
            {
                // Send UserData;
                if (player.UpdateHeroes())
                {
                    ServerSend.UserData(_fromClient, player);
                    Logger.Write($"[<<][{_fromClient}] Update UserData {player.heroes.Count}");

                    // Send to Tutorial or Village;
                    if (player.tutorialDone == 0)
                    {
                        var id = GameLogic.fights.Count + 1;
                        ServerSend.GoToTutorial(_fromClient, id);
                        Logger.Write($"[<<][{_fromClient}] Send User To Tutorial");
                        // Hardcode Tutorial Map to ID 1
                        GameLogic.fights.Add(id, new Fight(id, GameLogic.maps[1], _fromClient));
                    }
                    else
                    {
                        ServerSend.GoToVillage(_fromClient);
                        Logger.Write($"[<<][{_fromClient}] Send User To Village");
                    }
                } else
                {
                    ServerSend.CreateCharacter(_fromClient);
                    Logger.Write($"[<<][{_fromClient}] Send User To Select Character");
                }
            }
            else
            {
                ServerSend.CreateCharacter(_fromClient);
                Logger.Write($"[<<][{_fromClient}] Send User To Select Character");
            }

            Server.clients[_fromClient].player = player;

            if (_fromClient != _clientIdCheck)
            {
                Logger.Write($"Player (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

        }

        public static void SetPrepCharacter(int _fromClient, Packet _packet)
        {
            int _charId = _packet.ReadInt();
            int location_x = _packet.ReadInt();
            int location_y = _packet.ReadInt();

            var client = Server.clients[_fromClient];

            var playerHero = client.player.heroes.Find(p => p.id == _charId);

            var heroToPlace = new HeroObject();
            heroToPlace.FromPlayer(playerHero);

            heroToPlace.location = new Vector2(location_x, location_y);

            client.currentFight.SetHero(heroToPlace);

            Logger.Write($"Added Character {heroToPlace.ID} to Location: {heroToPlace.location.X}/{heroToPlace.location.Y}");
        }

        public static void ChangeReadyState(int _fromClient, Packet _packet)
        {
            var client = Server.clients[_fromClient];
            if(client.currentFight == null)
                return;

            bool _state = _packet.ReadBool();

            client.currentFight.changeClientState(client, _state);

            Logger.Write("Client changed ready state: " + _state.ToString());
        }

        public static void CreateCharacter(int _fromClient, Packet _packet)
        {
            int heroId = _packet.ReadInt();
            string nickname = _packet.ReadString();

            Logger.Write($"Creating {heroId} with Name: {nickname}");
            var player = Server.clients[_fromClient].player;

            DatabaseManager.createUser(nickname, player);
            DatabaseManager.getAccountByFacebookId(player);
            DatabaseManager.addHeroToPlayer(heroId, player);

            player.UpdateHeroes();
            ServerSend.UserData(_fromClient, player);

            var id = GameLogic.fights.Count() + 1;
            ServerSend.GoToTutorial(_fromClient, id);
            GameLogic.fights.Add(id, new Fight(id, _fromClient));
        }
    }
}
