using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Assets.Scripts.Battle;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BattleManager : UIPanel
{
    private int turn = 0;
    float turnTimeLeft = 0.0f;
    bool turnTimerWarningPlayd = false;

    public Map battleMap;
    public TeamID myTeam = TeamID.BLUE;
    
    public ActionState state;

    public bool ready = false;
    public bool enemyReady = false;
    public bool started = false;
      
    public static BattleManager Instance;
    
    public GameObject heroList;
    public GameObject actionBar;
    public GameObject prepBar;

    public GameObject waitingForAll;


    public GameObject _goTurnChange;
    public TMPro.TextMeshProUGUI _goTurnChangeText;
    public TMPro.TextMeshProUGUI enemyTurnLbl;
    public Button endTurnBtn;

    public Color greeny = new Color32(0x1C, 0xA4, 0x00, 0xFF);
    public Color bluey = new Color32(0x00, 0x1C, 0xA4, 0xFF);
    public Color redy = new Color32(0xA4, 0x00, 0x1C, 0xFF);

    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI turnText;

    List<PrepHeroItem> prepHeroItems = new List<PrepHeroItem>();

    public FieldHero selectedHero = null;
    public SpellData activeSpell = null;
    public TileObjState currentState = TileObjState.None;

    public Button cheatBtn;


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
        prepBar.SetActive(true);
        actionBar.SetActive(false);
        waitingForAll.SetActive(true);
        enemyTurnLbl.gameObject.SetActive(false);
        endTurnBtn.onClick.AddListener(() =>
        {
            ClientSend.EndTurn();
        });
        endTurnBtn.gameObject.SetActive(false);

        ClientSend.ClientLoaded();
        cheatBtn.onClick.AddListener(() =>
        {
            ClientSend.SendPacket("GCHEATWIN");
        });
    }

    public void EndGame(JObject packetResult)
    {
        state = ActionState.End;
        var test = UIManager.Instance.OpenPanel("EndOfGame");
        var eogPanel = test.GetComponent<EndOfGamePvE>();
        eogPanel.challengeAgainBtn.gameObject.SetActive(false);
        eogPanel.continueBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel("UIVillage", true);
            eogPanel.Close();
        });
        eogPanel.chapterLbl.text = "Test Game";
        // TOOD: Destory BattleManager, Go to Previous Screen or Village.
        
    }

    public void EndTurn(int turnTime)
    {
        turn++;
        _goTurnChangeText.text = "Turn " + turn;
        turnText.text = "Turn " + turn;
        _goTurnChange.GetComponent<Animator>().Play("TurnChangeText");
        if (state == ActionState.Preparation)
        {
            prepBar.SetActive(false);
            HideReadyText();
            ChangeState(ActionState.TeamBlue);
        }
        else if(state == ActionState.TeamBlue)
        {
            ChangeState(ActionState.TeamRed);
        }
        else if(state == ActionState.TeamRed)
        {
            ChangeState(ActionState.TeamBlue);
        }
        foreach(var heroList in battleMap.units)
        {
            foreach(var hero in heroList.Value)
            {
                hero.hasAttacked = false;
                hero.hasMoved = false;
            }
        }
        this.turnTimeLeft = turnTime;
    }

    public void AllLoaded()
    {
        if (started == true)
            return;
        waitingForAll.SetActive(false);
        _goTurnChangeText.text = "Preparing Phase";
        turnText.text = "Prepare Phase";
        _goTurnChange.SetActive(true);
        turnTimeLeft = 30.0f;
        this.started = true;
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

    public void Attack(int id, int spellId, int x, int y)
    {
        var caster = battleMap.GetFieldHeroById(id);
        caster.Attack(spellId, x, y);
    }

    public void SpawnUnit(Hero hero)
    {
        GameObject _gameObject = Instantiate(hero.playerHero.template.overwriteGameObject) as GameObject;
        var _heroScript = _gameObject.AddComponent<FieldHero>();
        _gameObject.transform.localScale -= new Vector3(0.6f, 0.6f);

        Debug.Log($"Spawned Hero is: {hero.Team}");

        if (hero.Team == myTeam)
            _gameObject.GetComponent<SpriteRenderer>().flipX = true;

        _heroScript.hero = hero;
        _heroScript.hero.renderObject = _gameObject;
        _heroScript.SetToCurrentPosition();

        battleMap.cells[(int)hero.location.x, (int)hero.location.y].objectOnTile = _heroScript.hero;

        battleMap.units[hero.Team].Add(_heroScript);
    }

    public void SendSetPrepHero(PlayerHero hero, Vector3 mouse, PrepHeroItem prepItem = null, TeamID team = TeamID.NEUTRAL)
    {
        var location = battleMap.GetTilePos(mouse);
        if (battleMap.IsInMap(location.x, location.y))
        {
            ClientSend.SetPrepCharacters(hero, location);
        }
    }

    public void GetSetPrepHero(int id, int x, int y)
    {
        if (battleMap.IsInMap(x, y))
        {
            var hero = Client.Instance.me.heroes.Find(h => h.id == id);
            SpawnCharacter(hero, x, y, TeamID.BLUE);
            Debug.LogError($"Spawn: {hero.template.name} at Level {hero.level}");
        }
    }

    public void SpawnCharacter(PlayerHero hero, int _x, int _y, TeamID team = TeamID.BLUE)
    {
        GameObject instance;
        if (hero.template.overwriteGameObject != null)
            instance = Instantiate(hero.template.overwriteGameObject) as GameObject;
        else
            instance = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
        var unit = instance.AddComponent<FieldHero>();
        instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{hero.template.id}/sprite");
        instance.transform.localScale -= new Vector3(0.6f, 0.6f);

        if (team == myTeam)
        {
            instance.GetComponent<SpriteRenderer>().flipX = true;
        }

        unit.hero = new Hero(hero);
        unit.SetPosition(_x, _y);
        unit.hero.renderObject = instance;
        battleMap.cells[_x, _y].objectOnTile = unit.hero;
        var charPos = unit.transform.position;
        charPos.y += 1.25f;

        var item = prepHeroItems.Find(h => h.hero == hero);
        if (item)
            item.SetDisabled(true);

        unit.hero.Team = team;
        battleMap.units[team].Add(unit);
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
            case ActionState.TeamBlue:
                battleMap.ClearColor();
                if (myTeam == TeamID.BLUE)
                {
                    endTurnBtn.gameObject.SetActive(true);
                    enemyTurnLbl.gameObject.SetActive(false);
                }
                else
                {
                    endTurnBtn.gameObject.SetActive(false);
                    enemyTurnLbl.gameObject.SetActive(true);
                }
                break;
            case ActionState.TeamRed:
                battleMap.ClearColor();
                if (myTeam == TeamID.RED)
                {
                    endTurnBtn.gameObject.SetActive(true);
                    enemyTurnLbl.gameObject.SetActive(false);
                }
                else
                {
                    endTurnBtn.gameObject.SetActive(false);
                    enemyTurnLbl.gameObject.SetActive(true);
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
        prepBar.SetActive(true);
        timerText = GameObject.Find("UI/Header/Turn/TurnTimer").GetComponent<TMPro.TextMeshProUGUI>();
        turnText = GameObject.Find("UI/Header/Turn/TurnText").GetComponent<TMPro.TextMeshProUGUI>();
        GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().onClick.AddListener(delegate {
            ready = !ready;
            ChangeReadyText("One");
            GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().interactable = false;
            ClientSend.ChangeReadyState(ready);
        });

        foreach (var hero in Client.Instance.me.heroes)
        {
            var heroListItem = Instantiate(Resources.Load("Prefabs/UI/Misc/HeroListItem"), heroList.transform) as GameObject;
            var prepItem = heroListItem.GetComponent<PrepHeroItem>();
            prepHeroItems.Add(prepItem);
            heroListItem.GetComponent<PrepHeroItem>().hero = hero;
            heroListItem.GetComponent<PrepHeroItem>().canDrag = true;
        }
        
        //AudioManager.Instance.PlayMusicWithCrossFade(Resources.Load<AudioClip>("Sounds/BGM/bgm_endlessstorm"));
    }
    
    void Update()
    {
        if (started)
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
            }
            else
            {
                turnTimerWarningPlayd = false;
            }
        
            battleMap.Update();

            clickEvent();
            idk();
        }
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
            actionBar.transform.Find("SelectedHeroInfo/Avatar/HeroListItem").GetComponent<PrepHeroItem>().SetupImagesByHeroData(selectedHero.hero.playerHero.template);
            actionBar.transform.Find("SelectedHeroInfo/Name").GetComponent<TMPro.TextMeshProUGUI>().text = selectedHero.hero.playerHero.template.name;
            int i = 1;
            foreach (var spell in selectedHero.hero.playerHero.spellData)
            {
                var slot = actionBar.transform.Find("SelectedHeroInfo/Spells/Slot" + i);
                if (slot != null)
                {
                    SkillBtn btn = slot.GetComponent<SkillBtn>();
                    btn.ChangeBtn(spell);
                }
                i++;
            }
        }
    }

    public override void Close()
    {
        for (int x = 0; x < battleMap.width; x++)
        {
            for (int y = 0; y < battleMap.height; y++)
            {
                if (battleMap.cells[x, y].objectOnTile != null)
                    Destroy(battleMap.cells[x, y].objectOnTile.renderObject);
            }
        }
        base.Close();
    }

    void clickEvent()
    {
        var mousePos = battleMap.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonUp(0) && battleMap.IsInMap(mousePos.x, mousePos.y) && currentState != TileObjState.Pending)
        {
            switch (state)
            {
                case ActionState.TeamRed:
                case ActionState.TeamBlue:
                    if (selectedHero != null)
                    {
                        if (selectedHero.hero.Team == myTeam)
                        {
                            //if (!selectedHero.hero.location.Equals(mousePos))
                            //{
                                if (currentState == TileObjState.Moving)
                                {
                                    if (battleMap._walkingTiles.Contains(mousePos))
                                    {
                                        if((myTeam == TeamID.BLUE && state == ActionState.TeamBlue) || (myTeam == TeamID.RED && state == ActionState.TeamRed))
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
                                        if ((myTeam == TeamID.BLUE && state == ActionState.TeamBlue) || (myTeam == TeamID.RED && state == ActionState.TeamRed))
                                            if (!selectedHero.hasAttacked)
                                                selectedHero.AttackReq(activeSpell.ID, mousePos.x, mousePos.y);
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
                            //}
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
                                    selectedHero.WantToAttack(Instance.activeSpell);
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
