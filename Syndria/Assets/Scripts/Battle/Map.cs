using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum ActionState
{
    Preparation, 
    None,
    Moving,
    Attacking,
    Pending,
}

public class Map : MonoBehaviour
{
    public Sprite background;

    public float timeLeft = 30.0f;
    public bool stop = true;

    private float seconds;

    private int width = 9, height = 5;
    public Tile[,] map;
    public Tilemap tileMap, highlightMap;
    public ActionState state;
    
    public List<Character> alliedCharacters;
    public List<Character> enemyCharacters;

    public GameObject prepObject;
    public List<PrepHeroItem> availableHeroes;

    private Character selectedCharacter;
    private SpellData selectedSpell;
    
    private Vector2Int hightlightedTile;

    private List<Vector2Int> _coloredCoordinates = new List<Vector2Int>();
    private Vector2Int _highlightedCoordinate = new Vector2Int();
    private BattleManager _battle;
    
    public TileBase _battleTile;
    public TileBase _highlightTile;

    public TMPro.TextMeshProUGUI timerText;

    void Start()
    {
        _battle = GetComponent<BattleManager>();
        _battleTile = Resources.Load("Battlefield/tile") as TileBase;
        _highlightTile = Resources.Load("Battlefield/tile_highlight") as TileBase;
        prepObject = GameObject.Find("Battlefield/UI/ActionBar/Preparation");

        foreach (var tiles in map)
        {
            SetTile(tileMap, tiles.coordinate, _battleTile);
        }
    }

    public void StartTimer(float from)
    {
        stop = false;
        timeLeft = from;
        Update();
        StartCoroutine(updateCoroutine());
    }

    private IEnumerator updateCoroutine()
    {
        while (!stop)
        {
            if(timerText == null)
                timerText = GameObject.Find("UI/TurnTimer").GetComponent<TMPro.TextMeshProUGUI>();
            timerText.text = string.Format("{0:00}", seconds);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Init()
    {
        tileMap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        highlightMap = GameObject.FindGameObjectWithTag("Highlight").GetComponent<Tilemap>();
        timerText = GameObject.Find("UI/TurnTimer").GetComponent<TMPro.TextMeshProUGUI>();
        GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().onClick.AddListener(delegate {
            ClientSend.SetPrepCharacters();
            //UIManager.instance.OpenMsgBox("Done.");
        });

        map = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = new Tile(x, y);
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
        StartTimer(30);
    }

    public List<Character> GetPrepCharacters()
    {
        List<Character> list = new List<Character>();
        foreach(var tile in map)
        {
            if (tile.characterOnTile)
            {
                list.Add(tile.characterOnTile);
            }
        }
        return list;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            timeLeft -= Time.deltaTime;
            
            seconds = timeLeft % 60;
            if (seconds < 0)
            {
                stop = true;
                seconds = 0;
            }
        }

        Vector3 mousePostition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePostition.x += 0.15f;
        mousePostition.y += 0.1f;
        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        Vector2Int destination = new Vector2Int(tile.x, tile.y);

        if (destination.x >= 0 && destination.x < width && destination.y >= 0 && destination.y < height)
        {
            if (destination != hightlightedTile)
            {
                SetHighlightTile(hightlightedTile, null);
            }
            SetHighlightTile(destination, _highlightTile);
        } else
            SetHighlightTile(hightlightedTile, null);

        if(state == ActionState.Preparation)
        {
            foreach (var prepTile in map)
            {
                if (prepTile.coordinate.x < 2)
                {
                    ColorTile(prepTile.coordinate, new Color32(0x1C, 0xA4, 0x00, 0xFF));
                }
            }
        } else
        {
            GameObject.Find("UI/ActionBar/Preparation").SetActive(false);
            foreach (var prepTile in map)
            {
                if (prepTile.coordinate.x < 2)
                {
                    ColorTile(prepTile.coordinate, new Color(0.2f,0.2f,0.2f,0.8f));
                }
            }
        }
    }

    public void SetState(ActionState newState)
    {
        state = newState;
    }

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

    public void SetHighlightTile(Vector2Int highlightPos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(highlightPos.x, highlightPos.y, 0);
        highlightMap.SetTileFlags(location, TileFlags.None);
        highlightMap.SetTile(location, tile);
        hightlightedTile = highlightPos;
    }

    public void SetTile(Tilemap layer, Vector2Int pos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
        layer.SetTileFlags(location, TileFlags.None);
        layer.SetTile(location, tile);
    }

    public Vector2Int GetTileByV3(Vector3 input)
    {
        Vector3 mousePostition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        return new Vector2Int(tile.x, tile.y);
    }

    public int ReverseX(int x)
    {
        return (width-1) - x;
    }

    public void SpawnCharacter(int id, Vector2Int location, bool isAllied, PrepHeroItem prepItem = null)
    {
        if (location.x >= 0 && location.x < width && location.y >= 0 && location.y < height)
        {
            var instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            instance.AddComponent<Character>();
            instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{id}_sprite");
            instance.transform.localScale -= new Vector3(0.6f, 0.6f);

            Character character = instance.GetComponent<Character>();

            if (isAllied)
            {
                instance.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                /*character = Instantiate(ninja.BattleSprite).AddComponent<Character>();
                enemyCharacters.Add(character);
                character.isPlayerCharacter = false;*/
            }

            //character.ninja = ninja;
            //character.battleMap = battleMap;

            Debug.Log($"Reversed X: " + ReverseX(location.x));
            ColorTile(new Vector2Int(ReverseX(location.x), location.y), Color.red);

            Debug.Log($"X:{location.x} Y:{location.y}");

            character.SetPosition(location.x, location.y);
            map[location.x, location.y].characterOnTile = character;
            var charPos = character.transform.position;
            charPos.y += 1.25f;

            if(prepItem != null)
            {
                prepItem.disabled = true;
            }

            //HealthBar healthBar = Instantiate(healthBarPrefab, charPos, Quaternion.identity, character.transform).GetComponentInChildren<HealthBar>();
            //healthBar.character = character;
        }
    }

}
