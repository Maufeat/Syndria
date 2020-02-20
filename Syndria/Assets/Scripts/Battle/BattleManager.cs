using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private int turn;
    public Map battleMap;
    
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

    // Start is called before the first frame update
    void Start()
    {
        battleMap = gameObject.AddComponent<Map>();
        battleMap.Init();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
