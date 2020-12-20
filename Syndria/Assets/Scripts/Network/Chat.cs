using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    private static Chat instance;

    public Button backdrop;

    public GameObject chatMsgPrefab;
    public GameObject scrollView;

    public static Chat Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        backdrop.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public void Hide()
    {
        GetComponent<Animator>().Play("Chat_Hide");
    }

    public void Show()
    {
        GetComponent<Animator>().Play("Chat_Show");
    }
}
