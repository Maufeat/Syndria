using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
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
        UIManager.Instance.OpenPanel("UILogin", true);
    }


    public void GooglePlaySignIn(UnityAction<bool> callback)
    {
        UIManager.Instance.OpenLoadingBox("Logging into Google Play Games ...");
#if !UNITY_EDITOR
        Social.Active.localUser.Authenticate((success, err) =>
        {
            if (success)
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        });
#else 
        callback(true);
#endif
    }

    public void Disconnect()
    {
        ThreadManager.ExecuteOnMainThread(() =>
        {
            UIManager.Instance.CloseAllPanel(true);
            UIManager.Instance.CloseLoadingBox();
            UIManager.Instance.OpenMsgBox("Disconnected");
            UIManager.Instance.OpenPanel("UILogin", true);
        });
    }

    void Update()
    {
    }

}
