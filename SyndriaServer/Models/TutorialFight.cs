using SyndriaServer.Enums;
using SyndriaServer.Utils;
using SyndriaServer.Utils.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SyndriaServer.Models
{
    public class TutorialFight
    {
        public int id;
        public int currentTurn = 0;
        public int turnTime = 5;

        public Map map;

        public List<Client> players = new List<Client>();

        public Timer turnTimer;
        
        public ActionState state;

        public List<PlayerHero> teamOneCharacters;
        public List<PlayerHero> teamTwoCharacters;

        public void SetTimer()
        {
            turnTimer = new Timer(turnTime * 1000);
            turnTimer.Elapsed += onTurnPassed;
            turnTimer.Enabled = true;
            turnTimer.AutoReset = false;
        }

        private void onTurnPassed(object sender, ElapsedEventArgs e)
        {
            switch (state) {
                case ActionState.Preparation:
                    currentTurn++;
                    state = ActionState.TeamOne;
                    ServerSend.EndTurn(players);
                    break;
                default:
                    break;
            }
            Logger.Write("Timer passed.");
        }

        public TutorialFight(int _id, int _playerOne, int _playerTwo)
        {
            id = _id;

            players.Add(Server.clients[_playerOne]);
            players.Add(Server.clients[_playerTwo]);
            players[0].currentFight = this;
            players[1].currentFight = this;

            map = new Map();
            map.SetAdjastance();
            state = ActionState.Preparation;
            SetTimer();
        }

        public TutorialFight(int _id, int _playerOne)
        {
            id = _id;

            players.Add(Server.clients[_playerOne]);
            players[0].currentFight = this;

            map = new Map();
            map.SetAdjastance();
            state = ActionState.Preparation;
            SetTimer();
        }

        public void SetCharacter(PlayerHero hero)
        {
            var tile = map.cells[Convert.ToInt32(hero.location.X), Convert.ToInt32(hero.location.Y)];
            tile.heroOnTle = hero;
            Logger.Write($"Set Character on: {tile.coordinate.X} / {tile.coordinate.Y}");

        }
    }
}
