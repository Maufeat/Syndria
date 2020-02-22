using System.Collections.Generic;
using System.Numerics;

namespace SyndriaServer.Models
{
    public class Tile
    {
        private bool _walkable = true;
        public bool walkable { get { return _walkable; } set { _walkable = value; } }
        public bool current = false;
        public bool target = false;
        public bool selectable = false;

        public Vector2 coordinate;

        public List<Tile> adjacencyList = new List<Tile>();

        //public  characterOnTile;

        //BFS 
        public bool visited = false;
        public Tile parent = null;
        public int distance = 0;
        
        public Tile(int x, int y)
        {
            coordinate = new Vector2(x, y);
        }
    }
}
