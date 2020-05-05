using SyndriaServer.Enums;
using System.Numerics;

namespace SyndriaServer.Interface
{
    public interface ITileObject
    {
        int ID { get; set; }
        string Name { get; set; }
        TeamID Team { get; set; }
        Vector2 location { get; }
        bool walkable { get; set; }

        // When walkable and someone walks into it.
        void Trigger();
    }
}
