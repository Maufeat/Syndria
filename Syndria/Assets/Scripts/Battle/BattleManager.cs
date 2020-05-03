using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private int turn = 1;
    float turnTimeLeft = 30.0f;
    bool turnTimerWarningPlayd = false;

    public Map battleMap;
    public TeamID myTeam = TeamID.BLUE;
    
    public ActionState state;

    public bool ready = false;
    public bool enemyReady = false;
      
    public static BattleManager Instance;
    
    public GameObject heroList;
    public GameObject actionBar;
    public GameObject prepBar;

    public GameObject _goTurnChange;
    public TMPro.TextMeshProUGUI _goTurnChangeText;

    public Color greeny = new Color32(0x1C, 0xA4, 0x00, 0xFF);
    public Color bluey = new Color32(0x00, 0x1C, 0xA4, 0xFF);
    public Color redy = new Color32(0xA4, 0x00, 0x1C, 0xFF);

    public TMPro.TextMeshProUGUI timerText;

    public FieldHero selectedHero = null;
    public SpellData activeSpell = null;
    public TileObjState currentState = TileObjState.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
    }
    
    void Start()
    {
        battleMap = new Map();
        battleMap.Init();
        Setup(); // Map Visuals and Audio
        ChangeState(ActionState.Preparation);
        prepBar = GameObject.Find("ActionBar/Preparation");
        actionBar = GameObject.Find("ActionBar/ActionBar");
        actionBar.SetActive(false);

        // Test Hero Placement
        /*var pounchingball = new PlayerHero()
        {
            ID = 9000,
            baseHeroData = Resources.Load<HeroData>("Characters/9000/data"),
            level = 1,
            aptitude = 0,
            xp = 0
        };

        SpawnCharacter(pounchingball, new Vector2Int(8, 2), false);*/

        ClientSend.ClientLoaded();
    }

    public void EndTurn()
    {
        if (state == ActionState.Preparation)
        {
            prepBar.SetActive(false);
            HideReadyText();
            actionBar.SetActive(true);
            _goTurnChange.GetComponent<Animator>().Play("TurnChangeText");
            _goTurnChangeText.text = "Turn " + turn;
            turn++;
            ChangeState(ActionState.TeamBlue);
        }
        else if(state == ActionState.TeamBlue)
        {
            ChangeState(ActionState.TeamRed);
        }
        else if(state == ActionState.TeamRed)
        {
            ChangeState(ActionState.TeamRed);
        }
    }

    public void AllLoaded()
    {
        _goTurnChangeText.text = "Preparing Phase";
        _goTurnChange.SetActive(true);
    }


    public void ChangeReadyState(bool ready)
    {
        ChangeReadyText("Two");
        enemyReady = ready;
    }

    public void MoveUnit(int id, int x, int y)
    {
        var unitToMove = battleMap.GetFieldHeroById(id);
        unitToMove.Move(x, y);
    }

    public void SpawnUnit(Hero hero)
    {
        GameObject _gameObject = Instantiate(hero.heroData.overwriteGameObject) as GameObject;
        var _heroScript = _gameObject.AddComponent<FieldHero>();
        _gameObject.transform.localScale -= new Vector3(0.6f, 0.6f);

        Debug.Log($"Spawned Hero is: {hero.Team}");

        if (hero.Team == myTeam)
            _gameObject.GetComponent<SpriteRenderer>().flipX = true;

        _heroScript.hero = hero;
        _heroScript.SetToCurrentPosition();

        battleMap.cells[(int)hero.location.x, (int)hero.location.y].objectOnTile = _heroScript.hero;

        battleMap.units[hero.Team].Add(_heroScript);
    }

    public void SpawnCharacter(PlayerHero hero, Vector3 mouse, bool isAllied, PrepHeroItem prepItem = null)
    {
        var location = battleMap.GetTilePos(mouse);
        if (battleMap.IsInMap(location.x, location.y))
        {
            GameObject instance;
            if (hero.baseHeroData.overwriteGameObject != null)
                instance = Instantiate(hero.baseHeroData.overwriteGameObject) as GameObject;
            else
                instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            var unit = instance.AddComponent<FieldHero>();
            instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{hero.baseHeroData.ID}/sprite");
            instance.transform.localScale -= new Vector3(0.6f, 0.6f);

            if (isAllied)
            {
                instance.GetComponent<SpriteRenderer>().flipX = true;
            }

            unit.hero = new Hero(hero);
            unit.SetPosition(location.x, location.y);
            battleMap.cells[location.x, location.y].objectOnTile = unit.hero;
            var charPos = unit.transform.position;
            charPos.y += 1.25f;

            if (prepItem != null)
            {
                prepItem.SetDisabled(true);
            }

            if (isAllied)
                battleMap.units[TeamID.BLUE].Add(unit);
            else
            {
                unit.hero.Team = TeamID.RED;
                battleMap.units[TeamID.RED].Add(unit);
            }

            if(isAllied)
                ClientSend.SetPrepCharacters(unit.hero);

            //HealthBar healthBar = Instantiate(healthBarPrefab, charPos, Quaternion.identity, character.transform).GetComponentInChildren<HealthBar>();
            //healthBar.character = character;
        }
    }


    public void SpawnCharacter(PlayerHero hero, Vector2Int location, bool isAllied, PrepHeroItem prepItem = null)
    {
        if (battleMap.IsInMap(location.x, location.y))
        {
            GameObject instance;
            if (hero.baseHeroData.overwriteGameObject != null)
                instance = Instantiate(hero.baseHeroData.overwriteGameObject) as GameObject;
            else
                instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            var unit = instance.AddComponent<FieldHero>();
            instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{hero.baseHeroData.ID}/sprite");
            instance.transform.localScale -= new Vector3(0.6f, 0.6f);
             
            if (isAllied)
            {
                instance.GetComponent<SpriteRenderer>().flipX = true;
            }

            unit.hero = new Hero(hero);
            unit.SetPosition(location.x, location.y);
            battleMap.cells[location.x, location.y].objectOnTile = unit.hero;
            var charPos = unit.transform.position;
            charPos.y += 1.25f;

            if (prepItem != null)
            {
                prepItem.SetDisabled(true);
            }

            if (isAllied)
                battleMap.units[TeamID.BLUE].Add(unit);
            else
            {
                unit.hero.Team = TeamID.RED;
                battleMap.units[TeamID.RED].Add(unit);
            }

            if (isAllied)
                ClientSend.SetPrepCharacters(unit.hero);

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
                        battleMap.WalkingTile(prepTile.coordinate);
                    }
                }
                break;
            default:
                battleMap.ClearColor();
                break;
        }
    }

    void ChangeReadyText(string _who)
    {
        GameObject.Find($"UI/Header/Ready{_who}").GetComponentInChildren<Image>().color = Color.blue;
        GameObject.Find($"UI/Header/Ready{_who}").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Ready";
    }

    void HideReadyText()
    {
        GameObject.Find($"UI/Header/ReadyOne").SetActive(false);
        GameObject.Find($"UI/Header/ReadyTwo").SetActive(false);
    }

    void Setup()
    {
        timerText = GameObject.Find("UI/Header/Turn/TurnTimer").GetComponent<TMPro.TextMeshProUGUI>();
        GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().onClick.AddListener(delegate {
            ready = !ready;
            ChangeReadyText("One");
            GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().interactable = false;
            ClientSend.ChangeReadyState(ready);
        });

        foreach (var hero in Client.instance.me.heroes)
        {
            var heroListItem = Instantiate(Resources.Load("Prefabs/UI/Misc/HeroListItem"), heroList.transform) as GameObject;
            heroListItem.GetComponent<PrepHeroItem>().hero = hero;
            heroListItem.GetComponent<PrepHeroItem>().canDrag = true;
        }
        
        AudioManager.Instance.PlayMusicWithCrossFade(Resources.Load<AudioClip>("Sounds/BGM/bgm_endlessstorm"));
    }
    
    void Update()
    {
        turnTimeLeft -= Time.deltaTime;
        
        if (turnTimeLeft >= 0)
        {
            timerText.text = Mathf.Round(turnTimeLeft).ToString();
            if (turnTimeLeft <= 3.5f && !turnTimerWarningPlayd)
            {
                turnTimerWarningPlayd = true;
                AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("Sounds/SFX/3210_countdown"));
            }
        } else
        {
            turnTimerWarningPlayd = false;
        }
        
        battleMap.Update();

        clickEvent();
        idk();
    }
    
    void idk()
    {
        if (selectedHero == null)
        {
            actionBar.SetActive(false);
        }
        else
        {
            actionBar.SetActive(true);
            actionBar.transform.Find("SelectedHeroInfo/Avatar/HeroListItem").GetComponent<PrepHeroItem>().hero = selectedHero.hero.playerHero;
            actionBar.transform.Find("SelectedHeroInfo/Avatar/HeroListItem").GetComponent<PrepHeroItem>().SetupImagesByHeroData(selectedHero.hero.heroData);
            actionBar.transform.Find("SelectedHeroInfo/Name").GetComponent<TMPro.TextMeshProUGUI>().text = selectedHero.hero.heroData.Name;
            int i = 1;
            foreach (var spell in selectedHero.hero.playerHero.spellData)
            {
                actionBar.transform.Find("SelectedHeroInfo/Spells/Slot" + i).GetComponent<SkillBtn>().ChangeBtn(spell);
            }
        }
    }

    void clickEvent()
    {
        var mousePos = battleMap.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0) && battleMap.IsInMap(mousePos.x, mousePos.y) && currentState != TileObjState.Pending)
        {
            switch (state)
            {
                case ActionState.TeamBlue:
                    if (selectedHero != null)
                    {
                        if (selectedHero.hero.Team == myTeam)
                        {
                            if (!selectedHero.hero.location.Equals(mousePos))
                            {
                                if (currentState == TileObjState.Moving)
                                {
                                    if (battleMap._walkingTiles.Contains(mousePos))
                                    {
                                        selectedHero.MoveReq(mousePos.x, mousePos.y);
                                        battleMap.ClearColor();
                                        return;
                                    }
                                    else
                                    {
                                        currentState = TileObjState.None;
                                    }
                                }
                                else if (currentState == TileObjState.Attacking)
                                {
                                    if (battleMap._attackingTiles.Contains(mousePos))
                                    {
                                        //selectedHero.SpellPreview();
                                    }
                                    else
                                    {
                                        currentState = TileObjState.None;
                                    }
                                    //TODO: Remove it when attack animation are created
                                    currentState = TileObjState.None;
                                }
                                battleMap.ClearColor();
                                selectedHero = null;
                                actionBar.SetActive(false);
                            }
                        } else
                        {
                            battleMap.ClearColor();
                            selectedHero = null;
                            actionBar.SetActive(false);
                        }
                    }
                    else
                    {
                        if (selectedHero == null && battleMap.cells[mousePos.x, mousePos.y].objectOnTile != null)
                        {
                            var objOnTile = battleMap.cells[mousePos.x, mousePos.y].objectOnTile;
                            if (objOnTile.GetType() == typeof(Hero))
                            {
                                if (objOnTile.Team == TeamID.BLUE && currentState == TileObjState.None)
                                {
                                    var movingHero = battleMap.units[TeamID.BLUE].Where(hero => hero.hero == objOnTile).First();
                                    if (!movingHero.hasMoved)
                                        movingHero.WantToMove();
                                    else
                                        movingHero.Select();
                                } else if (objOnTile.Team == TeamID.RED)
                                {
                                    battleMap.units[TeamID.RED].Where(hero => hero.hero == objOnTile).First().Select();
                                }
                            }
                        }
                    }
                    break;
            }
        }
        if (battleMap.IsInMap(mousePos.x, mousePos.y) && currentState != TileObjState.Pending)
        {
            switch (state)
            {
                case ActionState.TeamBlue:
                    if (selectedHero != null)
                    {
                        if (selectedHero.hero.Team == myTeam)
                        {
                            if (currentState == TileObjState.Attacking)
                            {
                                if (battleMap._rangeTiles.Contains(mousePos))
                                {
                                    selectedHero.SpellPreview(mousePos);
                                } else
                                {
                                    selectedHero.WantToAttack(BattleManager.Instance.activeSpell);
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
