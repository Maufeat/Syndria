using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models
{
    public class Map
    {
        private int width = 9, height = 5;
        public Tile[,] cells;

        
        public void SetAdjastance()
        {
            cells = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new Tile(x, y);
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x > 0)
                    {
                        cells[x, y].adjacencyList.Add(cells[x - 1, y]);
                    }
                    if (x < width - 1)
                    {
                        cells[x, y].adjacencyList.Add(cells[x + 1, y]);
                    }
                    if (y > 0)
                    {
                        cells[x, y].adjacencyList.Add(cells[x, y - 1]);
                    }
                    if (y < height - 1)
                    {
                        cells[x, y].adjacencyList.Add(cells[x, y + 1]);
                    }
                }
            }
        }
    }
}
