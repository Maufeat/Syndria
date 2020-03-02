using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models
{

    public class Fight
    {
        private int currentTurn = 0;
        private int currentTeamTurn = 0;
        private int width = 9, height = 5;

        public Player[] players = new Player[1];

        public int spectators;
        public Tile[,] map;
        //public ActionState state;

        //public List<Character> teamOneCharacters;
        //public List<Character> teamTwoCharacters;

        public Fight()
        {
            //state = ActionState.Preparation;
        }

        public void SetPlayers()
        {
            players[0] = new Player();
            players[1] = new Player();
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
