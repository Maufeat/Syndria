using Assets.Scripts.Models.Village;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Village : UIPanel
{    
    private float camVertExtent;
    private float halfWidth;

    public Image ProfilePicture;

    // Camera Drag
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    void Start()
    {
        keepOnDispose = true;

        AudioManager.Instance.PlayMusicWithFade(Resources.Load<AudioClip>("Sounds/BGM/bgm_hol"), 1);
        //StartCoroutine(NetworkManager.Instance.DownloadImage(NetworkManager.Instance.fbPic, GameObject.Find("ProfilePicture").GetComponent<Image>()));

        GameObject.Find("NameLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.nickname;
        GameObject.Find("LevelLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.level.ToString();
        GameObject.Find("XPLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.exp.ToString() + " / " + Client.Instance.me.experienceTable[Client.Instance.me.level];
        GameObject.Find("Diamonds").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.diamonds.ToString();
        GameObject.Find("Gold").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.gold.ToString();
        var sprite = Resources.Load<Sprite>("Characters/"+Client.Instance.me.profile_picture_id+"/thumb");
        if (sprite != null)
            ProfilePicture.sprite = sprite;

        GameObject.Find("ProfilePicture").GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel("UIAccountSettings");
        });
    }

    public void onDiscordButton()
    {
        Application.OpenURL("https://discord.gg/6wFphds");
    }

    public void testBtn()
    {
        ClientSend.Test();
    }

    public void onChatClick()
    {
        Chat.Instance.Show();
    }
}
