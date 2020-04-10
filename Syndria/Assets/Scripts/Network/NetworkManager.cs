using Facebook.Unity;
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
    [Header("SDK Initialized")]
    public bool IsSDKReady = false;
    [Header("Login Status")]
    public bool AmILoggedIn = false;
    [Header("User Details")]
    public AccessToken FBAccessToken;

    public string fbPic;
    public string fbName;

    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        QualitySettings.antiAliasing = 2;
        instance = this;

        if (!FB.IsLoggedIn)
        {
            FB.Init(OnFBSDKInit, null, null);
        }
        else if (FB.IsLoggedIn)
        {
            AmILoggedIn = true;
        }

        Application.runInBackground = true;
    }


    public void Connect()
    {
        LogIn(true);
    }

    public void Disconnect()
    {
        FB.LogOut();

        UIManager.instance.CloseAllPanel(true);
        UIManager.instance.CloseLoadingBox();
        UIManager.instance.OpenMsgBox("Disconnected.");
        UIManager.instance.uiLogin.SetActive(true);

        Debug.Log("Disconnected");
    }

    void Update()
    {
        /*
        if (client.Connected)
        {
            Telepathy.Message msg;

            while (client.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:

                        UIManager.instance.OpenLoadingBox("Logging in...");

                        Debug.Log("Connected");

                        me = new Player();

                        LittleEndianWriter writer = new LittleEndianWriter();
                        writer.WriteShort((short)PacketCmd.C2S_FBLogin);
                        writer.WriteSizedString(FBAccessToken.TokenString);
                        client.Send(writer.Data);

                        break;
                    case Telepathy.EventType.Data:
                        LittleEndianReader reader = new LittleEndianReader(msg.data);
                        PacketCmd cmd = (PacketCmd)reader.ReadShort();
                        switch (cmd)
                        {
                            case PacketCmd.S2C_UserDataToVillage:
                                me.UpdatePlayerData(reader);
                                UIManager.instance.CloseAllPanel();
                                UIManager.instance.CloseLoadingBox();
                                UIManager.instance.OpenPanel("UIVillage", true);
                                break;
                            case PacketCmd.S2C_CreateCharacter:
                                UIManager.instance.CloseAllPanel();
                                UIManager.instance.CloseLoadingBox();
                                UIManager.instance.OpenPanel("UICreateCharacter", true);
                                break;
                            case PacketCmd.S2C_GoToTutorial:
                                UIManager.instance.CloseAllPanel();
                                UIManager.instance.CloseLoadingBox();
                                UIManager.instance.OpenPanel("BattleField", true);
                                UIManager.instance.OpenMsgBox("Welcome to Tutorial XyZ \n <color=red>red text test</color>");
                                break;
                        }
                        break;
                    case Telepathy.EventType.Disconnected:

                        FB.LogOut();
                        UIManager.instance.CloseAllPanel();
                        UIManager.instance.CloseLoadingBox();
                        UIManager.instance.uiLogin.SetActive(true);

                        Debug.Log("Disconnected");
                        break;
                }
            }
        }
        else
        {
            Telepathy.Message msg;
            while (client.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Disconnected:

                        FB.LogOut();
                        UIManager.instance.CloseAllPanel();
                        UIManager.instance.CloseLoadingBox();
                        UIManager.instance.uiLogin.SetActive(true);
                        UIManager.instance.OpenMsgBox("Failed to connect to game servers. Please try again later");

                        break;
                }
            }
        }*/
    }

    private void OnFBSDKInit()
    {
        if (FB.IsInitialized)
        {
            IsSDKReady = true;
            FacebookLog("Ready!", true, false, false);
        }
        else
        {
            IsSDKReady = false;
            FacebookLog("Failed To Initialize", false, true, false);
        }
    }

    public void LogIn(bool IsClicked)
    {
        FacebookLog("Logging....", true, false, false);
        List<string> fbPerms = new List<string>() {
            "public_profile",
            "email",
            "user_friends"
        };

        if (IsClicked)
        {
            UIManager.instance.OpenLoadingBox("Connecting to Facebook...");
            FB.LogInWithReadPermissions(fbPerms, OnLoggedIn);
        }
    }

    private void OnLoggedIn(ILoginResult _LoginResponse)
    {
        if (_LoginResponse.Error != null)
        {
            FacebookLog("Failed To LOG IN", false, true, false);
            Debug.Log(_LoginResponse.Error);
        }
        else
        {
            AmILoggedIn = FB.IsLoggedIn;
            if (AmILoggedIn)
            {
                FacebookLog("Logged In", true, false, false);
                FBAccessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                
                RequestUserInfo();
            }
            else
            {
                FacebookLog("User Cancelled", false, true, false);
            }
        }
    }

    public void RequestUserInfo()
    {
        List<string> myPerms = new List<string>();

        foreach (string perm in FBAccessToken.Permissions)
        {
            switch (perm)
            {
                case "public_profile":
                    myPerms.Add("name");
                    myPerms.Add("first_name");
                    myPerms.Add("last_name");
                    myPerms.Add("picture.type(large)");
                    break;
                case "email":
                    myPerms.Add("email");
                    break;
            }
        }
        if (!myPerms.Contains("email"))
            FacebookLog("EMAIL : Permission Required", false, true, false);

        if (!myPerms.Contains("name") && !myPerms.Contains("first_name")
            && !myPerms.Contains("last_name") && !myPerms.Contains("picture"))
            FacebookLog("PUBLIC PROFILE : Permission Required", false, true, false);

        string myPermsStr = string.Join(",", myPerms.ToArray());
        Debug.Log(myPermsStr);
        FB.API("/me?fields=" + myPermsStr, HttpMethod.GET, OnUserInfoGrabbed);
    }

    private void OnUserInfoGrabbed(IResult _FbUserResp)
    {
        if (_FbUserResp.Error == null)
        {
            UIManager.instance.uiLogin.SetActive(false);

            UIManager.instance.OpenLoadingBox("Connecting to Game Server...");

            IDictionary picture = _FbUserResp.ResultDictionary["picture"] as IDictionary;
            IDictionary data = picture["data"] as IDictionary;
            fbPic = data["url"] as String;
            fbName = _FbUserResp.ResultDictionary["name"].ToString();

            //GameObject.Find("Debug/Text").GetComponent<TMPro.TextMeshProUGUI>().text = _FbUserResp.ResultDictionary["name"].ToString();
            //StartCoroutine(DownloadImage(data["url"] as String));

            Client.instance.ConnectToServer();

            //StartCoroutine(ServerManager.Instance.LogIn(myFbId, myName, myEmail));
        }
        else
        {
            Debug.Log(_FbUserResp.Error);
        }
    }


    public IEnumerator DownloadImage(string mediaUrl, Image place)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            place.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }

    public void FacebookLog(string _msg, bool IsLog, bool IsError, bool IsWarning)
    {
        if (IsLog)
        {
            Debug.Log("<facebook> " + _msg);

        }
        else if (IsError)
        {
            UIManager.instance.CloseLoadingBox();
            UIManager.instance.OpenMsgBox(_msg);
            Debug.LogError("<facebook> " + _msg);
        }
        else
        {
            Debug.LogWarning("<facebook> " + _msg);
        }
    }
}
