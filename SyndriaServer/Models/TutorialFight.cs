using SyndriaServer.Utils;
using SyndriaServer.Utils.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models
{
    public class TutorialFight
    {
        public int id;
        private int currentTurn = 0;
        private int currentTeamTurn = 0;

        public int turnTime = 30;

        private DateTime lastTurn;

        private int width = 9, height = 5;

        public Client player;

        public int spectators;
        public Tile[,] map;
        public ActionState state;

        public List<Character> teamOneCharacters;
        public List<Character> teamTwoCharacters;

        public TutorialFight(int _id, int _playerId)
        {
            id = _id;
            SetPlayers(Server.clients[_playerId]);
            SetAdjastance();
            state = ActionState.Preparation;
            lastTurn = DateTime.Now;
        }

        public void Update()
        {
            TimeSpan span = (DateTime.Now - lastTurn);

            // 30 Seconds time after each action.
            if(span.Seconds >= turnTime)
            {
                lastTurn = DateTime.Now;
                ServerSend.OpenMessageBox(player.id, "Turn passed.");
                Console.WriteLine("Next Turn.");
            }
        }

        public void SetPrepChars(List<Character> chars)
        {
            foreach (var chara in chars)
            {
                var tile = map[Convert.ToInt32(chara.location.X), Convert.ToInt32(chara.location.Y)];
                tile.characterOnTile = chara;
                Logger.Write($"Set Character on: {tile.coordinate.X} / {tile.coordinate.Y}");
            }
        }

        public void SetPlayers(Client _player)
        {
            player = _player;
            //player.id
        }
        
        public void SetAdjastance()
        {
            map = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new Tile(x, y);
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x > 0)
                    {
                        map[x, y].adjacencyList.Add(map[x - 1, y]);
                    }
                    if (x < width - 1)
                    {
                        map[x, y].adjacencyList.Add(map[x + 1, y]);
                    }
                    if (y > 0)
                    {
                        map[x, y].adjacencyList.Add(map[x, y - 1]);
                    }
                    if (y < height - 1)
                    {
                        map[x, y].adjacencyList.Add(map[x, y + 1]);
                    }
                }
            }
        }
    }
}
