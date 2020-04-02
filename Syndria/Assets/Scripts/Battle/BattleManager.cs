using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private int turn = 1;
    float turnTimeLeft = 5.0f;

    public Map battleMap;
    
    public ActionState state;
    
    public static BattleManager instance;

    public GameObject heroList;

    public TMPro.TextMeshProUGUI timerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
    }
    
    void Start()
    {
        if (!Client.instance.isConnected)
            SetupFakePlayer();
        battleMap = new Map();
        battleMap.Init();
        Setup();
        ChangeState(ActionState.Preparation);
    }

    public void EndTurn()
    {
        if (state == ActionState.Preparation)
        {
            GameObject.Find("ActionBar/Preparation").SetActive(false);
            GameObject.Find("TurnChange").GetComponent<Animator>().Play("TurnChangeText");
            GameObject.Find("TurnChangeText").GetComponent<TMPro.TextMeshProUGUI>().text = "Turn " + turn;
            turn++;
            ChangeState(ActionState.TeamOne);
        }
        else if(state == ActionState.TeamOne)
        {
            ChangeState(ActionState.TeamTwo);
        }
        else if(state == ActionState.TeamTwo)
        {
            ChangeState(ActionState.TeamTwo);
        }
    }

    void SetupFakePlayer()
    {
        /*var player = new Player();
        player.Username = "Maufeat";
        player.level = 1;

        var testUnit = new PlayerHero();
        var testUnit2 = new PlayerHero();
        testUnit.hero.heroData = Resources.Load<HeroData>("Characters/2/data");
        testUnit2.hero.heroData = Resources.Load<HeroData>("Characters/3/data");

        player.heroes = new List<PlayerHero>()
        {
            testUnit,
            testUnit2
        };

        Client.instance.me = player;*/

    }


    public void SpawnCharacter(HeroData id, Vector3 mouse, bool isAllied, PrepHeroItem prepItem = null)
    {
        var location = battleMap.GetTilePos(mouse);
        if (location.x >= 0 && location.x < battleMap.width && location.y >= 0 && location.y < battleMap.height)
        {
            var instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            var unit = instance.AddComponent<FieldHero>();
            instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{id.ID}/sprite");
            instance.transform.localScale -= new Vector3(0.6f, 0.6f);

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

            unit.player = new PlayerHero();
            unit.player.hero = new Hero();
            unit.player.hero.heroData = id;
            unit.SetPosition(location.x, location.y);
            battleMap.cells[location.x, location.y].objectOnTile = unit.player.hero;
            var charPos = unit.transform.position;
            charPos.y += 1.25f;

            if (prepItem != null)
            {
                prepItem.SetDisabled(true);
            }

            ClientSend.SetPrepCharacters(unit.player);

            //HealthBar healthBar = Instantiate(healthBarPrefab, charPos, Quaternion.identity, character.transform).GetComponentInChildren<HealthBar>();
            //healthBar.character = character;
        }
    }

    void ChangeState(ActionState _state)
    {
        state = _state;
        switch (state)
        {
            case ActionState.Preparation:
                foreach (var prepTile in battleMap.cells)
                {
                    if (prepTile.coordinate.x < 2)
                    {
                        battleMap.ColorTile(prepTile.coordinate, new Color32(0x1C, 0xA4, 0x00, 0xFF));
                    }
                }
                break;
            default:
                battleMap.ClearColor();
                break;
        }
    }

    void Setup()
    {
        //tileMap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        //highlightMap = GameObject.FindGameObjectWithTag("Highlight").GetComponent<Tilemap>();
        timerText = GameObject.Find("UI/Header/Turn/TurnTimer").GetComponent<TMPro.TextMeshProUGUI>();
        GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().onClick.AddListener(delegate {
            GameObject.Find("TurnChange").GetComponent<Animator>().Play("TurnChangeText");
            GameObject.Find("TurnChangeText").GetComponent<TMPro.TextMeshProUGUI>().text = "Turn " + turn;
            turn++;
        });

        foreach (var hero in Client.instance.me.heroes)
        {
            //Debug.Log($"Instantiate: { hero.hero.heroData.name }");
            var heroListItem = Instantiate(Resources.Load("Prefabs/UI/Misc/HeroListItem"), heroList.transform) as GameObject;
            heroListItem.GetComponent<PrepHeroItem>().id = hero.hero.heroData.ID;
            heroListItem.GetComponent<PrepHeroItem>().hero = hero.hero.heroData;
            heroListItem.GetComponent<PrepHeroItem>().canDrag = true;
        }
    }
    
    void Update()
    {
        turnTimeLeft -= Time.deltaTime;
        timerText.text = Mathf.Round(turnTimeLeft).ToString();

        battleMap.Update();
    }
}
