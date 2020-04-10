using UnityEngine;

public interface ITileObject
{
    int ID { get; }
    TeamID Team { get; }
    Vector2Int location{ get; }
    bool walkable { get; }
    GameObject renderObject { get; }

    // When walkable and someone walks into it.
    void Trigger();
}