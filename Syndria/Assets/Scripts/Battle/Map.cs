using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Map
{
    public Sprite background;
    
    private int width = 9, height = 5;

    public Tile[,] map;
    public Tilemap tileMap, highlightMap;
    public ActionState state;
    
    public List<Character> alliedCharacters;
    public List<Character> enemyCharacters;

    public List<PrepHeroItem> availableHeroes;

    private Character selectedCharacter;
    private SpellData selectedSpell;
    
    private Vector2Int hightlightedTile;

    private List<Vector2Int> _coloredCoordinates = new List<Vector2Int>();
    private Vector2Int _highlightedCoordinate = new Vector2Int();
    
    public TileBase _battleTile;
    public TileBase _highlightTile;
    public GameObject _prepObject;

    public TMPro.TextMeshProUGUI timerText;

    void Start()
    {
        _prepObject = GameObject.Find("Battlefield/UI/ActionBar/Preparation");

    }

    // Draw Tiles and Set Tilemap;
    public void Init()
    {
        _battleTile = Resources.Load("Battlefield/tile") as TileBase;
        _highlightTile = Resources.Load("Battlefield/tile_highlight") as TileBase;

        tileMap = GameObject.Find("Stage/Grid/Tilemap").GetComponent<Tilemap>();
        highlightMap = GameObject.Find("Stage/Grid/Highlight").GetComponent<Tilemap>();

        map = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile newTile = new Tile(x, y);
                map[x, y] = newTile;
                SetTile(tileMap, newTile.coordinate, _battleTile);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x > 0)
                {
                    map[x, y].adjacencyList.Add(map[x - 1, y]);
                }
                if (x < width - 1)
                {
                    map[x, y].adjacencyList.Add(map[x + 1, y]);
                }
                if (y > 0)
                {
                    map[x, y].adjacencyList.Add(map[x, y - 1]);
                }
                if (y < height - 1)
                {
                    map[x, y].adjacencyList.Add(map[x, y + 1]);
                }
            }
        }
        //StartTimer(30);
    }

    public List<TileObject> GetPrepCharacters()
    {
        List<TileObject> list = new List<TileObject>();
        foreach(var tile in map)
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
        mousePostition.x += 0.15f;
        mousePostition.y += 0.1f;
        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        Vector2Int destination = new Vector2Int(tile.x, tile.y);

        if (destination.x >= 0 && destination.x < width && destination.y >= 0 && destination.y < height)
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

    public void ColorTiles(List<Vector2Int> tilesToColor, Color color)
    {
        foreach (Vector2Int coordinate in tilesToColor)
        {
            Vector3Int location = new Vector3Int(coordinate.x, coordinate.y, 0);
            tileMap.SetTileFlags(location, TileFlags.None);
            tileMap.SetColor(location, color);
            _coloredCoordinates.Add(coordinate);
        }
    }

    public void ColorTile(Vector2Int tileToColor, Color color)
    {
        Vector3Int location = new Vector3Int(tileToColor.x, tileToColor.y, 0);
        tileMap.SetTileFlags(location, TileFlags.None);
        tileMap.SetColor(location, color);
        _coloredCoordinates.Add(tileToColor);
    }

    public void ClearColor()
    {
        foreach (Vector2Int coordinate in _coloredCoordinates)
        {
            tileMap.SetColor(new Vector3Int(coordinate.x, coordinate.y, 0), Color.white);
        }
        _coloredCoordinates.Clear();
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
        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        return new Vector2Int(tile.x, tile.y);
    }

    #endregion

    public void WantToMove(Character character)
    {
        /*ClearColor();
        state = MapState.Moving;
        characterSelected = character;
        var adTiles = character.characterMovement.GetWalkableTiles(characterSelected, character.ninja.Stats.MovementRange);
        var coords = new List<Vector2Int>();
        foreach (Tile tile in adTiles)
        {
            coords.Add(tile.coordinate);
        }
        ColorTiles(coords, Color.green);*/
    }
    
    /*public void SpawnCharacter(int id, Vector2Int location, bool isAllied, PrepHeroItem prepItem = null)
    {
        
        if (location.x >= 0 && location.x < width && location.y >= 0 && location.y < height)
        {
            var instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            instance.AddComponent<Character>();
            instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{id}_sprite");
            instance.transform.localScale -= new Vector3(0.6f, 0.6f);

            Hero character = instance.GetComponent<Hero>();

            if (isAllied)
            {
                instance.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                //character = Instantiate(ninja.BattleSprite).AddComponent<Character>();
                //enemyCharacters.Add(character);
                //character.isPlayerCharacter = false;
            }

            character.SetPosition(location.x, location.y);
            map[location.x, location.y].objectOnTile = character;
            var charPos = character.transform.position;
            charPos.y += 1.25f;

            if(prepItem != null)
            {
                prepItem.disabled = true;
            }

            //HealthBar healthBar = Instantiate(healthBarPrefab, charPos, Quaternion.identity, character.transform).GetComponentInChildren<HealthBar>();
            //healthBar.character = character;
        }
    }*/
}
