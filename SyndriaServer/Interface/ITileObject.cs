using SyndriaServer.Enums;
using System.Numerics;

namespace SyndriaServer.Interface
{
    public interface ITileObject
    {
        int ID { get; }
        string Name { get; }
        string Description { get; }
        TeamID Team { get; }
        Vector2 location { get; }
        bool walkable { get; }

        // When walkable and someone walks into it.
        void Trigger();
    }
}
