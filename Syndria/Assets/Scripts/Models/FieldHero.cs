using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldHero : MonoBehaviour
{
    public Hero hero;

    public float x_offset_fixed = -7.35f;
    public float y_offset_fixed = -2.45f;
    public float x_offset_multi = 1.85f;
    public float y_offset_multi = 1.5f;

    public bool hasAttacked = false;
    public bool hasMoved = false;

    public bool isMoving;
    public Stack<Vector3> waypoints;
    public Stack<Vector3> totalWaypoints;
    public Vector3 target;
    public float moveSpeed = 5f;
    
    private float blurAmount = 0;
    private bool blurActive;

    private Material material;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public void SetPosition(float x, float y)
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        hero.location = new Vector2(x, y);
        transform.position = new Vector3(hero.location.x * x_offset_multi + x_offset_fixed, hero.location.y * y_offset_multi + y_offset_fixed, 5);
        spriteRenderer.sortingOrder = (99 - Convert.ToInt32(hero.location.y));
    }

    public void SetToCurrentPosition()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(hero.location.x * x_offset_multi + x_offset_fixed, hero.location.y * y_offset_multi + y_offset_fixed, 5);
        spriteRenderer.sortingOrder = (99 - Convert.ToInt32(hero.location.y));
    }

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if(hasAttacked && hasMoved && BattleManager.Instance.state == ActionState.TeamBlue)
        {
            spriteRenderer.material.SetInt("_IsGray", 0);
        } else
        {
            spriteRenderer.material.SetInt("_IsGray", 1);
        }

        blurAmount = Mathf.Clamp(blurAmount, 0f, 1.5f);
        spriteRenderer.material.SetFloat("_BlurAmount", blurAmount);
        if (blurActive)
        {
            blurAmount += 25f * Time.deltaTime;
        }
        else
        {
            blurAmount -= 25f * Time.deltaTime;
        }

        if (isMoving)
        {
            blurActive = true;
            if (totalWaypoints.Count >= 3)
                moveSpeed = 7.5f;
            
            Vector3 movePosition = Vector3.MoveTowards(new Vector3(hero.location.x, hero.location.y), target, Time.deltaTime * moveSpeed);
            if (movePosition.x >= target.x - 0.01f && movePosition.x <= target.x + 0.01f && movePosition.y >= target.y - 0.01f && movePosition.y <= target.y + 0.01f)
            {
                movePosition.x = target.x;
                movePosition.y = target.y;
                if (waypoints.Count > 0)
                {
                    target = waypoints.Pop();
                }
                else
                {
                    blurActive = false;
                    isMoving = false;
                    totalWaypoints = new Stack<Vector3>();
                    moveSpeed = 5f;
                    animator.SetBool("Run", false);
                    if (!hasAttacked)
                    {
                        BattleManager.Instance.currentState = TileObjState.Attacking;
                        BattleManager.Instance.selectedHero = this;
                    }
                    else
                        BattleManager.Instance.currentState = TileObjState.None;
                }
            }
            SetPosition(movePosition.x, movePosition.y);
        }
    }

    public void Move(int x, int y)
    {
        totalWaypoints = new Stack<Vector3>();
        waypoints = new Stack<Vector3>();

        BattleManager.Instance.battleMap.RemoveUnit(hero.location);
        BattleManager.Instance.battleMap.AddUnit(hero, new Vector2Int(x, y));

        Tile tile = BattleManager.Instance.battleMap.cells[x, y];

        while (tile != null)
        {
            totalWaypoints.Push(new Vector3(tile.coordinate.x, tile.coordinate.y));
            waypoints.Push(new Vector3(tile.coordinate.x, tile.coordinate.y));
            tile = tile.parent;
        }

        isMoving = true;

        animator.SetBool("Run", true);
        target = waypoints.Pop();
        spriteRenderer.color = new Color(255, 255, 255, 155);
        hasMoved = true;
        BattleManager.Instance.currentState = TileObjState.Pending;
    }

    public void WantToMove()
    {
        BattleManager.Instance.battleMap.ClearColor();
        BattleManager.Instance.currentState = TileObjState.Moving;
        BattleManager.Instance.selectedHero = this;
        var adTiles = GetWalkableTiles((int)hero.heroData.BaseStats.Movement);
        var coords = new List<Vector2Int>();
        foreach (Tile tile in adTiles)
        {
            coords.Add(tile.coordinate);
        }
        BattleManager.Instance.battleMap.ColorTiles(coords, BattleManager.Instance.greeny);
    }

    public void Select()
    {
        BattleManager.Instance.battleMap.ClearColor();
        BattleManager.Instance.selectedHero = this;
    }

    public List<Tile> GetWalkableTiles(int distance)
    {
        Queue<Tile> open = new Queue<Tile>();
        List<Tile> visited = new List<Tile>();

        BattleManager.Instance.battleMap.ResetVisited();

        Tile startTile = BattleManager.Instance.battleMap.GetTileByCoordinate(hero.location);
        startTile.distance = 0;

        open.Enqueue(startTile);

        while (open.Count > 0)
        {
            Tile tile = open.Dequeue();
            visited.Add(tile);
            tile.visited = true;
            if (tile.distance < distance)
            {
                foreach (Tile neighbourTile in tile.adjacencyList)
                {
                    if (!neighbourTile.visited && neighbourTile.isWalkable())
                    {
                        neighbourTile.distance = tile.distance + 1;
                        neighbourTile.parent = tile;
                        open.Enqueue(neighbourTile);
                    }
                }
            }
        }
        visited.RemoveAt(0);
        return visited;
    }
}
