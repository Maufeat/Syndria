using UnityEngine;

public class TileObject : ITileObject
{
    public int ID { get; set; }

    public TeamID Team { get; }

    public Vector2Int location { get; set; }

    public bool walkable { get; }

    public GameObject renderObject { get; }

    public void Trigger()
    {
        throw new System.NotImplementedException();
    }
}
