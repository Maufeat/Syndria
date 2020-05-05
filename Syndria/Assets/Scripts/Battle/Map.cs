using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Map
{
    public Sprite background;
    
    public int width = 9, height = 5;

    public Tile[,] cells;
    public Tilemap tileMap, highlightMap;
    
    public Dictionary<TeamID, List<FieldHero>> units = new Dictionary<TeamID, List<FieldHero>>();

    public List<PrepHeroItem> availableHeroes;
    
    private Vector2Int hightlightedTile;

    public List<Vector2Int> _walkingTiles = new List<Vector2Int>();
    public List<Vector2Int> _rangeTiles = new List<Vector2Int>();
    public List<Vector2Int> _attackingTiles = new List<Vector2Int>();

    public TileBase _battleTile;
    public TileBase _highlightTile;
    public GameObject _prepObject;

    public TMPro.TextMeshProUGUI timerText;

    void Start()
    {
        _prepObject = GameObject.Find("Battlefield/UI/ActionBar/Preparation");
    }

    /// <summary>Initializes the Map. Draws Tiles and Set Tilemap.</summary>
    public void Init()
    {
        _battleTile = Resources.Load("Battlefield/tile") as TileBase;
        _highlightTile = Resources.Load("Battlefield/tile_highlight") as TileBase;

        tileMap = GameObject.Find("Stage/Grid/Tilemap").GetComponent<Tilemap>();
        highlightMap = GameObject.Find("Stage/Grid/Highlight").GetComponent<Tilemap>();

        units = new Dictionary<TeamID, List<FieldHero>>
        {
            { TeamID.BLUE, new List<FieldHero>() },
            { TeamID.RED, new List<FieldHero>() },
            { TeamID.NEUTRAL, new List<FieldHero>() },
        };

        cells = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile newTile = new Tile(x, y);
                cells[x, y] = newTile;
                SetTile(tileMap, newTile.coordinate, _battleTile);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x > 0)
                {
                    cells[x, y].adjacencyList.Add(cells[x - 1, y]);
                }
                if (x < width - 1)
                {
                    cells[x, y].adjacencyList.Add(cells[x + 1, y]);
                }
                if (y > 0)
                {
                    cells[x, y].adjacencyList.Add(cells[x, y - 1]);
                }
                if (y < height - 1)
                {
                    cells[x, y].adjacencyList.Add(cells[x, y + 1]);
                }
            }
        }
        //StartTimer(30);
    }

    public void ResetVisited()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y].visited = false;
                cells[x, y].parent = null;
            }
        }
    }

    public FieldHero GetFieldHeroById(int id)
    {
        foreach(var team in units)
        {
            foreach(var unit in team.Value)
            {
                if (unit.hero.ID == id)
                    return unit;
            }
        }

        return null;
    }

    public bool IsInMap(int x, int y)
    {
        return (x >= 0 && x < width && y < height && y >= 0);
    }

    public Tile GetTileByCoordinate(Vector2 coordinate)
    {
        return cells[(int)coordinate.x, (int)coordinate.y];
    }

    public void AddUnit(AttackableObject character, Vector2Int position)
    {
        cells[position.x, position.y].objectOnTile = character;
    }

    public void RemoveUnit(Vector2 position)
    {
        cells[(int)position.x, (int)position.y].objectOnTile = null;
    }

    public List<TileObject> GetPrepCharacters()
    {
        List<TileObject> list = new List<TileObject>();
        foreach(var tile in cells)
        {
            if (tile.objectOnTile == null)
            {
                list.Add(tile.objectOnTile);
            }
        }
        return list;
    }

    public void Update()
    {
        Vector3 mousePostition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Tile offsets, because else it would look "more" choppy
        mousePostition.x += 0.15f;
        mousePostition.y += 0.1f;

        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        Vector2Int destination = new Vector2Int(tile.x, tile.y);

        if (IsInMap(destination.x, destination.y))
        {
            if (destination != hightlightedTile)
            {
                SetHighlight(hightlightedTile, null);
            }
            SetHighlight(destination, _highlightTile);
        } else
            SetHighlight(hightlightedTile, null);
    }


    #region Tile Utils

    public void WalkingTiles(List<Vector2Int> tilesToColor)
    {
        foreach (Vector2Int coordinate in tilesToColor)
        {
            Vector3Int location = new Vector3Int(coordinate.x, coordinate.y, 0);
            tileMap.SetTileFlags(location, TileFlags.None);
            tileMap.SetColor(location, BattleManager.Instance.greeny);
            _walkingTiles.Add(coordinate);
        }
    }

    public void WalkingTile(Vector2Int tileToColor)
    {
        Vector3Int location = new Vector3Int(tileToColor.x, tileToColor.y, 0);
        tileMap.SetTileFlags(location, TileFlags.None);
        tileMap.SetColor(location, BattleManager.Instance.greeny);
        _walkingTiles.Add(tileToColor);
    }

    public void RangeTiles(List<Vector2Int> tilesToColor)
    {
        foreach (Vector2Int coordinate in tilesToColor)
        {
            Vector3Int location = new Vector3Int(coordinate.x, coordinate.y, 0);
            tileMap.SetTileFlags(location, TileFlags.None);
            tileMap.SetColor(location, BattleManager.Instance.bluey);
            _rangeTiles.Add(coordinate);
        }
    }

    public void AttackingTiles(List<Vector2Int> tilesToColor)
    {
        foreach (Vector2Int coordinate in tilesToColor)
        {
            Vector3Int location = new Vector3Int(coordinate.x, coordinate.y, 0);
            tileMap.SetTileFlags(location, TileFlags.None);
            tileMap.SetColor(location, BattleManager.Instance.redy);
            _attackingTiles.Add(coordinate);
        }
    }

    public void ClearColor()
    {
        foreach (Vector2Int coordinate in _walkingTiles)
        {
            tileMap.SetColor(new Vector3Int(coordinate.x, coordinate.y, 0), new Color(0.2f, 0.2f, 0.2f, 0.8f));
        }
        foreach (Vector2Int coordinate in _rangeTiles)
        {
            tileMap.SetColor(new Vector3Int(coordinate.x, coordinate.y, 0), new Color(0.2f, 0.2f, 0.2f, 0.8f));
        }
        foreach (Vector2Int coordinate in _attackingTiles)
        {
            tileMap.SetColor(new Vector3Int(coordinate.x, coordinate.y, 0), new Color(0.2f, 0.2f, 0.2f, 0.8f));
        }

        _walkingTiles.Clear();
        _rangeTiles.Clear();
        _attackingTiles.Clear();
    }
    
    public void ClearColorForSpellPreview()
    {
        foreach (Vector2Int coordinate in _walkingTiles)
        {
            tileMap.SetColor(new Vector3Int(coordinate.x, coordinate.y, 0), new Color(0.2f, 0.2f, 0.2f, 0.8f));
        }
        foreach (Vector2Int coordinate in _rangeTiles)
        {
            tileMap.SetColor(new Vector3Int(coordinate.x, coordinate.y, 0), new Color(0.2f, 0.2f, 0.2f, 0.8f));
        }

        _walkingTiles.Clear();
        _rangeTiles.Clear();
    }

    public void SetHighlight(Vector2Int pos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
        highlightMap.SetTileFlags(location, TileFlags.None);
        highlightMap.SetTile(location, tile);
        hightlightedTile = pos;
    }

    public void SetTile(Tilemap layer, Vector2Int pos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
        layer.SetTileFlags(location, TileFlags.None);
        layer.SetTile(location, tile);
    }

    public Vector2Int GetTilePos(Vector3 input)
    {
        Vector3 mousePostition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePostition.x += 0.15f;
        mousePostition.y += 0.1f;
        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        return new Vector2Int(tile.x, tile.y);
    }

    #endregion
}
