using SyndriaServer.Models.FightData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models
{
    public class Map
    {
        private int width = 9, height = 5;
        public Tile[,] cells;

        public Map()
        {
            SetAdjastance();
        }
        
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
        
        public Tile GetTileByCoordinate(Vector2 coordinate)
        {
            return cells[(int)coordinate.X, (int)coordinate.Y];
        }

        public bool IsInMap(int x, int y)
        {
            return (x >= 0 && x < width && y < height && y >= 0);
        }
        
        public List<Tile> GetTilesByPattern(Vector2 center, SpellPattern pattern)
        {
            List<Tile> tiles = new List<Tile>();

            Tile startTile = GetTileByCoordinate(center);

            int x_offset = pattern._width / 2;
            int y_offset = pattern._height / 2;

            for (int x = 0; x < pattern._width; x++)
            {
                for (int y = 0; y < pattern._height; y++)
                {
                    if (pattern.GetData(x, y) > 0)
                    {
                        var relative_map_position_x = ((x_offset - pattern._width) + center.X) + x + 1;
                        var relative_map_position_y = ((y_offset - pattern._height) + center.Y) + y + 1;
                        if (IsInMap((int)relative_map_position_x, (int)relative_map_position_y))
                        {
                            tiles.Add(GetTileByCoordinate(new Vector2(relative_map_position_x, relative_map_position_y)));
                        }
                    }
                }
            }

            return tiles;
        }
    }
}
