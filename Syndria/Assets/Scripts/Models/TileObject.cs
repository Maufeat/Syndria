using UnityEngine;

public class TileObject : ITileObject
{
    public int ID { get; set; }

    public TeamID Team { get; set; }

    public Vector2 location { get; set; }

    public bool walkable { get; }

    public GameObject renderObject { get; set;  }

    public TileObjState mapState { get; set; }

    public void Trigger()
    {
        throw new System.NotImplementedException();
    }
}
