using Newtonsoft.Json;
using SyndriaServer.Enums;
using SyndriaServer.Models.FightData;
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
    public class Fight
    {
        public int id;
        public int currentTurn = 0;
        public float turnTime = 30.0f;

        public Map map;

        public List<Client> players = new List<Client>();

        public Timer turnTimer;
        
        public ActionState state;

        private MapData mapData;

        public Dictionary<TeamID, List<HeroObject>> units = new Dictionary<TeamID, List<HeroObject>>();

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

        public void changeClientState(Client client, bool readyState)
        {
            client.readyState = readyState;
            bool allReady = true;

            foreach (var _c in players)
            {
                if(_c != client)
                    ServerSend.ChangeReadyState(_c, readyState);
                if (_c.readyState == false)
                    allReady = false;
            }

            if(allReady && state == ActionState.Preparation)
                ServerSend.EndTurn(players);
        }

        // Solo PvE
        public Fight(int _id, MapData data, int _playerOne)
        {
            id = _id;
            mapData = data;

            units.Add(TeamID.BLUE, new List<HeroObject>());
            units.Add(TeamID.RED, new List<HeroObject>());
            units.Add(TeamID.NEUTRAL, new List<HeroObject>());

            players.Add(Server.clients[_playerOne]);
            players[0].currentFight = this;

            map = new Map();
            state = ActionState.Preparation;
            

            // WAIT FOR PLAYER READY STATE
            foreach(var enemy in mapData.mobs)
            {
                HeroObject _enemy = new HeroObject()
                {
                    ID = (units[TeamID.RED].Count + 1),
                    location = new Vector2(enemy.x, enemy.y),
                    baseHero = GameLogic.heroes[enemy.baseHeroId],
                    IsDead = false,
                    Team = TeamID.RED
                };

                ServerSend.SpawnUnit(players, _enemy);
            }

            //SetTimer();
        }

        public Fight(int _id, int _playerOne)
        {
            id = _id;

            players.Add(Server.clients[_playerOne]);
            players[0].currentFight = this;

            map = new Map();
            map.SetAdjastance();
            state = ActionState.Preparation;
            // Since it's PvE, tell Client that PlayerTwo is ready
            ServerSend.ChangeReadyState(players[0], true);
            SetTimer();
        }

        public void SetHero(HeroObject hero)
        {
            var tile = map.cells[Convert.ToInt32(hero.location.X), Convert.ToInt32(hero.location.Y)];
            tile.objectOnTile = hero;
            Logger.Write($"Set Character on: {tile.coordinate.X} / {tile.coordinate.Y}");
        }

        public void MoveHero(HeroObject hero, int _x, int _y)
        {
            var tile = map.cells[Convert.ToInt32(hero.location.X), Convert.ToInt32(hero.location.Y)];
            map.cells[_x, _y].objectOnTile = tile.objectOnTile;
            tile.objectOnTile = null;
            Logger.Write($"Moved Character to: {_x} / {_y}");
        }
    }
}
