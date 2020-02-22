using UnityEngine;

public class TileObject : ITileObject
{
    public int ID { get; }

    public string Name { get; }

    public string Description { get; }

    public TeamID Team { get; }

    public Vector2Int location { get; set; }

    public bool walkable { get; }

    public GameObject renderObject { get; }

    public void Trigger()
    {
        throw new System.NotImplementedException();
    }
}
