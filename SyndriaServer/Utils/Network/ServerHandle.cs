﻿using SyndriaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var player = Server.clients[_fromClient].player;
            player = new Player();
            player.fbToken = fbToken;

            if (DatabaseManager.getAccountByFacebookId(player))
            {
                // Send UserData;
                ServerSend.UserData(_fromClient, player);
                Logger.Write($"[<<][{_fromClient}] Update UserData");

                // Send to Tutorial or Village;
                if (player.tutorialDone == 0)
                {
                    var id = GameLogic.fights.Count() + 1;
                    ServerSend.GoToTutorial(_fromClient, id);
                    Logger.Write($"[<<][{_fromClient}] Send User To Tutorial");
                    GameLogic.fights.Add(id, new Models.TutorialFight(_fromClient, id));
                }
                else
                {
                    ServerSend.GoToVillage(_fromClient);
                    Logger.Write($"[<<][{_fromClient}] Send User To Village");
                }
            }
            else
            {
                var id = GameLogic.fights.Count() + 1;
                ServerSend.GoToTutorial(_fromClient, id);
                Logger.Write($"[<<][{_fromClient}] Send User To Tutorial");
                GameLogic.fights.Add(id, new Models.TutorialFight(_fromClient, id));
            }

            if (_fromClient != _clientIdCheck)
            {
                Logger.Write($"Player (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }


        }

        public static void SetPrepCharacters(int _fromClient, Packet _packet)
        {
            int _fightId = _packet.ReadInt();
            int _prepLength = _packet.ReadInt();
            
            var x = GameLogic.fights[_fightId];
            List<Character> chars = new List<Character>();

            for(int i = 0; i < _prepLength; i++)
            {
                Character c = new Character();
                c.ID = _packet.ReadInt();
                c.location.X = _packet.ReadInt();
                c.location.Y = _packet.ReadInt();
                Logger.Write($"Added Character {c.ID} to Location: {c.location.X}/{c.location.Y}");
            }
        }
    }
}
