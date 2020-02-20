using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile
{
    private bool _walkable = true;
    public bool walkable { get { return characterOnTile == null && _walkable; } set { _walkable = value; } }
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public Vector2Int coordinate;

    public List<Tile> adjacencyList = new List<Tile>();

    public Character characterOnTile;

    //BFS 
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public Tile(int x, int y)
    {
        coordinate = new Vector2Int(x, y);
    }

    void Update()
    {
        /*if (current)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else if (target)
            GetComponent<SpriteRenderer>().color = Color.green;
        else if (selectable)
            GetComponent<SpriteRenderer>().color = Color.red;
        else
            GetComponent<SpriteRenderer>().color = Color.white;*/
    }

    public void Reset()
    {
        walkable = true;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;

        adjacencyList.Clear();
    }

    public void FindNeighbors()
    {
        Reset();
        //Vector2 v2 = new Vector2(GetComponent<Coordinate>().X, GetComponent<Coordinate>().Y);
    }

    public void CheckTile(Vector2 direction)
    {

    }
}