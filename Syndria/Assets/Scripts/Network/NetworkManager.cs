using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;

    public PlayGamesPlatform platform;

    public static NetworkManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        Application.runInBackground = true;
    }

    public void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false)
                .RequestIdToken()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }
    }


    public async void Connect()
    {
#if !UNITY_EDITOR
        Social.Active.localUser.Authenticate((success, err) =>
        {
            if (success)
            {
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    UIManager.Instance.uiLogin.SetActive(false);
                    UIManager.Instance.OpenLoadingBox("Logging in...");
                });
                Client.Instance.ConnectToServer();
            }
            else
            {
                //this.level.text = "Failed to login " + err;
            }
        });
#else 
        ThreadManager.ExecuteOnMainThread(() =>
        {
            UIManager.Instance.uiLogin.SetActive(false);
            UIManager.Instance.OpenLoadingBox("Logging in...");
        });
        await Client.Instance.ConnectToServer();
#endif
    }

    public void Disconnect()
    {
        ThreadManager.ExecuteOnMainThread(() =>
        {
            UIManager.Instance.CloseAllPanel(true);
            UIManager.Instance.CloseLoadingBox();
            UIManager.Instance.OpenMsgBox("Disconnected");
            UIManager.Instance.uiLogin.SetActive(true);
        });
    }

    void Update()
    {
    }

}
