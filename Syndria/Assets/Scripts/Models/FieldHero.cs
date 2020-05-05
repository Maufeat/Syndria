using Assets.Scripts.Battle;
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

    public void Attack(int id, int x, int y)
    {
        foreach (var spell in hero.playerHero.spellData)
        {
            if (spell.ID == id)
            {
                Vector2 castPos = new Vector2(x, y);
                animator.Play("Attack");
                SpellHolder holder = spell.prefab.GetComponent<SpellHolder>();
                holder.spell.spellData = spell;
                holder.spell.Cast(castPos, GetTilesByPattern(castPos, spell.attackPattern));
                //spell.Cast(castPos, GetTilesByPattern(castPos, spell.attackPattern));
            }
        }
    }

    public void MoveReq(int x, int y)
    {
        ClientSend.MoveUnit(hero.ID, x, y);
    }

    public void AttackReq(int spellId, int x, int y)
    {
        //Debug.Log($"Attack Req: {spellId} {x}/{y}");
        ClientSend.Attack(hero.ID, spellId, x, y);
    }

    public void WantToAttack(SpellData _spell)
    {
        BattleManager.Instance.battleMap.ClearColor();
        BattleManager.Instance.currentState = TileObjState.Attacking;
        BattleManager.Instance.selectedHero = this;
        BattleManager.Instance.activeSpell = _spell;
        var adTiles = GetTilesByPattern(hero.location, _spell.rangePattern);
        var coords = new List<Vector2Int>();
        foreach (Tile tile in adTiles)
        {
            coords.Add(tile.coordinate);
        }
        BattleManager.Instance.battleMap.RangeTiles(coords);
    }

    public void SpellPreview(Vector2 location)
    {
        WantToAttack(BattleManager.Instance.activeSpell);
        var adTiles = GetTilesByPattern(location, BattleManager.Instance.activeSpell.attackPattern);
        var coords = new List<Vector2Int>();
        foreach (Tile tile in adTiles)
        {
            coords.Add(tile.coordinate);
        }
        BattleManager.Instance.battleMap.AttackingTiles(coords);
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
        BattleManager.Instance.battleMap.WalkingTiles(coords);
    }

    public void Select()
    {
        BattleManager.Instance.battleMap.ClearColor();
        BattleManager.Instance.selectedHero = this;
    }

    public List<Tile> GetTilesByPattern(Vector2 center, SpellPattern pattern)
    {
        List<Tile> tiles = new List<Tile>();

        Tile startTile = BattleManager.Instance.battleMap.GetTileByCoordinate(center);

        int x_offset = pattern._width / 2;
        int y_offset = pattern._height / 2;

        for (int x = 0; x < pattern._width; x++)
        {
            for (int y = 0; y < pattern._height; y++)
            {
                if(pattern.GetData(x,y) > 0)
                {
                    var relative_map_position_x = ((x_offset - pattern._width) + center.x) + x + 1;
                    var relative_map_position_y = ((y_offset - pattern._height) + center.y) + y + 1;
                    if (BattleManager.Instance.battleMap.IsInMap((int)relative_map_position_x, (int)relative_map_position_y))
                    {
                        tiles.Add(BattleManager.Instance.battleMap.GetTileByCoordinate(new Vector2(relative_map_position_x, relative_map_position_y)));
                    }
                }
            }
        }

        return tiles;
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
