using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIFormation : UIPanel
{
    public Tile[,] cells;
    public Tilemap tileMap, highlightMap;
    public TileBase _battleTile;
    public TileBase _highlightTile;

    public Button closeBtn;
    private Vector2Int hightlightedTile;

    public GameObject heroList;
    public List<GameObject> pool;

    // Start is called before the first frame update
    void Start()
    {
        _battleTile = Resources.Load("Battlefield/tile") as TileBase;
        _highlightTile = Resources.Load("Battlefield/tile_highlight") as TileBase;
        tileMap = GameObject.Find("FormationTilemap").GetComponent<Tilemap>();
        tileMap.GetComponent<TilemapRenderer>().sortingOrder = 10;
        highlightMap = GameObject.Find("FormationHighlight").GetComponent<Tilemap>();
        highlightMap.GetComponent<TilemapRenderer>().sortingOrder = 20;

        closeBtn.onClick.AddListener(() => {
            Close();
        });

        Client.Instance.onHeroListDrag += Instance_onHeroListDrag;

        Init();
    }

    public override void Close()
    {
        foreach(var obj in pool)
        {
            Destroy(obj);
        }
        Client.Instance.onHeroListDrag -= Instance_onHeroListDrag;
        base.Close();
    }

    private void AddHeroByLocation(PlayerHero hero, Vector2Int location, bool init = false)
    {
        if (IsInMap(location.x, location.y) && cells[location.x, location.y].objectOnTile == null)
        {
            GameObject instance;
            if (hero.template.overwriteGameObject != null)
                instance = Instantiate(hero.template.overwriteGameObject) as GameObject;
            else
                instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            var unit = instance.AddComponent<FieldHero>();
            instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{hero.template.id}/sprite");
            instance.transform.localScale -= new Vector3(0.6f, 0.6f);
            instance.GetComponent<SpriteRenderer>().flipX = true;
            unit.hero = new Hero(hero);
            unit.x_offset_fixed = 4.0f;
            unit.y_offset_fixed = -3.25f;
            unit.x_offset_multi = 1.85f;
            unit.y_offset_multi = 1.5f;
            unit.SetPosition(location.x, location.y);
            cells[location.x, location.y].objectOnTile = unit.hero;
            pool.Add(instance);
            var charPos = unit.transform.position;
            charPos.y += 1.25f;
            if (!init)
            {
                foreach (PrepHeroItem component in heroList.GetComponentsInChildren<PrepHeroItem>())
                {
                    if (component.hero == hero)
                    {
                        component.SetDisabled(true);
                    }
                }
            }
            ClientSend.SetFormation(hero, location);
        }
    }

    private void Instance_onHeroListDrag(PlayerHero hero, Vector3 mousePos)
    {
        var location = GetTilePos(mousePos);
        AddHeroByLocation(hero, location);
    }

    public bool IsInMap(int x, int y)
    {
        return (x >= 0 && x < Client.Instance.me.currentFormation.width && y < Client.Instance.me.currentFormation.height && y >= 0);
    }


    void Init()
    {
        Debug.Log($"Make Height {Client.Instance.me.currentFormation.height} und {Client.Instance.me.currentFormation.width} Width");
        cells = new Tile[Client.Instance.me.currentFormation.width, Client.Instance.me.currentFormation.height];

        var tempList = new List<PlayerHero>();

        if (Client.Instance.me.currentFormation != null)
        {
            int i = 0;
            for (int x = 0; x < Client.Instance.me.currentFormation.width; x++)
            {
                for (int y = 0; y < Client.Instance.me.currentFormation.height; y++)
                {
                    Tile newTile = new Tile(x, y);
                    cells[x, y] = newTile;
                    SetTile(tileMap, newTile.coordinate, _battleTile);
                    if (Client.Instance.me.currentFormation.heroes[x, y] != null)
                    {
                        tempList.Add(Client.Instance.me.currentFormation.heroes[x, y]);
                        AddHeroByLocation(Client.Instance.me.currentFormation.heroes[x, y], new Vector2Int(x, y), true);
                    }
                    i++;
                }
            }
        }

        foreach (var hero in Client.Instance.me.heroes)
        {
            var heroListItem = Instantiate(Resources.Load("Prefabs/UI/Misc/HeroListItem"), heroList.transform) as GameObject;
            heroListItem.GetComponent<PrepHeroItem>().hero = hero;
            heroListItem.GetComponent<PrepHeroItem>().canDrag = true;
            heroListItem.GetComponent<PrepHeroItem>().SetupImages();
            if (tempList.Contains(hero))
            {
                heroListItem.GetComponent<PrepHeroItem>().SetDisabled(true);
            }
        }
    }

    public void ResetFormation()
    {
        foreach (var obj in pool)
        {
            Destroy(obj);
        }
        int i = 0;
        for (int x = 0; x < Client.Instance.me.currentFormation.width; x++)
        {
            for (int y = 0; y < Client.Instance.me.currentFormation.height; y++)
            {
                cells[x, y].objectOnTile = null;
                Client.Instance.me.currentFormation.heroes[x, y] = null;
                i++;
            }
        }

        foreach (Transform listItem in heroList.transform)
        {
            listItem.GetComponent<PrepHeroItem>().SetDisabled(false);
        }

        ClientSend.DeleteFormation();
    }

    public void SetTile(Tilemap layer, Vector2Int pos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
        layer.SetTile(location, tile);
        layer.SetTileFlags(location, TileFlags.None);
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
        }
        else
            SetHighlight(hightlightedTile, null);
    }

    public void SetHighlight(Vector2Int pos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
        highlightMap.SetTileFlags(location, TileFlags.None);
        highlightMap.SetTile(location, tile);
        hightlightedTile = pos;
    }

    public Vector2Int GetTilePos(Vector3 input)
    {
        Vector3 mousePostition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePostition.x += 0.15f;
        mousePostition.y += 0.1f;
        Vector3Int tile = tileMap.WorldToCell(mousePostition);
        return new Vector2Int(tile.x, tile.y);
    }
}
