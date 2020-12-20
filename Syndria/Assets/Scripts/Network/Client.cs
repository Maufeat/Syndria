using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using NativeWebSocket;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Advertisements;

public class Client : MonoBehaviour, IUnityAdsListener
{
    public static Client Instance;

    public Player me;

    public int v = 100;

    public int[] availableCreateCharacter = { 1 };

    private WebSocket websocket;
    private delegate void PacketHandler(string _packet);
    private static Dictionary<string, PacketHandler> packetHandlers;


    public delegate void HeroListDrag(PlayerHero hero, Vector3 mousePos);
    public event HeroListDrag onHeroListDrag;

    //ads
#if UNITY_IOS
    private string gameId = "3937122";
#elif UNITY_ANDROID
    private string gameId = "3937123";
#endif

    string myPlacementId = "rewardedVideo";

#if UNITY_EDITOR
    bool testMode = true;
#else
    bool testMode = false;
#endif
    public void OnHeroListDrag(PlayerHero hero, Vector3 mousePos)
    {
        onHeroListDrag.Invoke(hero, mousePos);
    }

    private async void OnApplicationQuit()
    {
        NetworkManager.Instance.Disconnect();
        await websocket.Close();
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(websocket != null)
            if (websocket.State == WebSocketState.Open)
                websocket.DispatchMessageQueue();
#endif
    }

    public async Task ConnectToServer(string host, int port)
    {
        var connectionString = $"ws://{host}:{port}";

        if (websocket == null)
            websocket = new WebSocket(connectionString);

        if (websocket.State == WebSocketState.Connecting || websocket.State == WebSocketState.Open)
            return;

        InitializeClientData();
        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };
        websocket.OnError += (e) =>
        {
            Debug.Log("Error!");
            ThreadManager.ExecuteOnMainThread(() =>
            {
                UIManager.Instance.CloseLoadingBox();
                UIManager.Instance.OpenMsgBox(e);
            });
        };
        websocket.OnClose += (e) =>
        {
            NetworkManager.Instance.Disconnect();
            websocket = new WebSocket(connectionString);
        };
        websocket.OnMessage += (bytes) =>
        {
            try
            {
                var packetAsString = Encoding.UTF8.GetString(bytes);
                var packetAsJson = JObject.Parse(packetAsString);
                Debug.Log(Convert.ToString((string)packetAsJson["msgHeader"]));
                packetHandlers[Convert.ToString((string)packetAsJson["msgHeader"])](packetAsString);
            } catch(Exception e)
            {

            }
        };
        await websocket.Connect();
    }

    public async void Send(string json)
    {
        await websocket.SendText(json);
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<string, PacketHandler>()
        {
            { "HC", ClientHandle.Welcome },
            { "SC", ClientHandle.UpdateServerConfig },
            { "MB", ClientHandle.MessageBox },
            { "CA", ClientHandle.CreateCharacter },
            { "TT", ClientHandle.GoToTutorial },
            { "HF", ClientHandle.UpdateHeroFormation },
            { "VA", ClientHandle.GoToVillage },
            { "AD", ClientHandle.UpdateUserData },
            { "HL", ClientHandle.UpdateHeroList },
            { "GPC", ClientHandle.SetPrepChar },
            { "GSL", ClientHandle.GameStateLoaded },
            { "GSR", ClientHandle.ChangeReadyState },
            { "FS", ClientHandle.StartFight },
            { "GST", ClientHandle.ChangeTurn },
            { "GAM", ClientHandle.MoveUnit },
            { "GAS", ClientHandle.Attack },
            { "GATEST", ClientHandle.GATEST },
            { "GSC", ClientHandle.SpawnUnit },
            { "GEG", ClientHandle.EndGameResult },
            { "AWV", ClientHandle.PleaseUpdate },
            { "UI", ClientHandle.UpdateInventory },
            { "GAR", ClientHandle.ActionResponse }
        };

        Debug.Log("Initialized packets.");
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady(myPlacementId))
        {
            Advertisement.Show(myPlacementId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    } 

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
