using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public int turn;
    public Map battleMap;

    public ActionState state;

    public static BattleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    
    void Start()
    {
        battleMap = new Map();
        battleMap.Init();
        Setup();
        ChangeState(ActionState.Preparation);
    }

    void ChangeState(ActionState _state)
    {
        state = _state;
        switch (state)
        {
            case ActionState.Preparation:
                foreach (var prepTile in battleMap.map)
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
        //timerText = GameObject.Find("UI/TurnTimer").GetComponent<TMPro.TextMeshProUGUI>();
        GameObject.Find("UI/ActionBar/Preparation/DoneBtn").GetComponent<Button>().onClick.AddListener(delegate {
            ClientSend.SetPrepCharacters();
        });
    }
    
    void Update()
    {
        battleMap.Update();
    }
}
