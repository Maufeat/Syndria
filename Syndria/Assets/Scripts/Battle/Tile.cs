using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile
{
    public bool walkable = true;

    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public Vector2Int coordinate;

    public List<Tile> adjacencyList = new List<Tile>();

    public TileObject objectOnTile;

    //BFS 
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public Tile(int x, int y)
    {
        coordinate = new Vector2Int(x, y);
    }

    public bool isWalkable()
    {
        if (!walkable)
            return false;

        if(objectOnTile != null)
            if (!objectOnTile.walkable)
                return false;

        return true;
    }

    void Update()
    {

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