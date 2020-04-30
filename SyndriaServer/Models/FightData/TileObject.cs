using SyndriaServer.Enums;
using SyndriaServer.Interface;
using System.Numerics;

namespace SyndriaServer.Models.FightData
{
    public class TileObject : ITileObject
    {
        public int ID { get; set; }

        public TeamID Team { get; set; }

        public Vector2 location { get; set; }

        public bool walkable { get; }

        public void Trigger()
        {
            throw new System.NotImplementedException();
        }
    }

}
