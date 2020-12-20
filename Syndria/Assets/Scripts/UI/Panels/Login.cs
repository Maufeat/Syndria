using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : UIPanel
{
    public Button loginBtn;

    public GameObject selectServer;

    public JObject fullServerConfig;
    public Dictionary<int, JObject> serverList = new Dictionary<int, JObject>();
    public int selectedServer;

    void Start()
    {
        AudioManager.Instance.PlayMusicWithFade(Resources.Load<AudioClip>("Sounds/BGM/bgm_tetsu"), 0);
        AudioManager.Instance.SetMusicVolume(0.2f);

        selectServer.SetActive(false);

        loginBtn.onClick.AddListener(() =>
        {
            NetworkManager.Instance.GooglePlaySignIn(async (success) =>
            {
                UIManager.Instance.CloseLoadingBox();
                if (!success)
                {
                    UIManager.Instance.OpenMsgBox("Couldn't sign in. Please try again later.");
                    return;
                }
                loginBtn.gameObject.SetActive(false);
                UIManager.Instance.OpenLoadingBox("Fetch Server List...");
                StartCoroutine(GetRequest("https://pockie.ninja/server.php"));
            });
        });
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            UIManager.Instance.CloseLoadingBox();
            if (webRequest.isNetworkError)
            {
                UIManager.Instance.OpenMsgBox("Error: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                var selectServerBtn = selectServer.transform.Find("CurrentServer").GetComponent<Button>();
                var serverName = selectServerBtn.transform.Find("ServerName").GetComponent<TMPro.TextMeshProUGUI>();
                var status = selectServerBtn.transform.Find("Status").GetComponent<Image>();
                var connectBtn = selectServer.transform.Find("ConnectBtn").GetComponent<Button>();

                fullServerConfig = JObject.Parse(webRequest.downloadHandler.text);

                foreach(JObject server in fullServerConfig["server_info"]["servers"])
                {
                    serverList.Add(Convert.ToInt32(server["id"]), server);
                }

                selectServer.SetActive(true);
                selectedServer = Convert.ToInt32(fullServerConfig["server_info"]["last_login"]);
                serverName.text = serverList[selectedServer]["name"].ToString();
                selectServerBtn.onClick.AddListener(() => {
                    UIManager.Instance.OpenPanel("SelectServer");
                });
                connectBtn.onClick.AddListener(async () =>
                {
                    UIManager.Instance.OpenLoadingBox("Connecting to the server...");
                    var serverAddress = serverList[selectedServer]["host"].ToString();
                    var serverPort = Convert.ToInt32(serverList[selectedServer]["port"]);
                    await Client.Instance.ConnectToServer(serverAddress, serverPort);
                });
            }
        }
    }

    public void ChangeSelectedServer(int id)
    {
        selectedServer = id;
        selectServer.transform.Find("CurrentServer").Find("ServerName").GetComponent<TMPro.TextMeshProUGUI>().text = serverList[selectedServer]["name"].ToString();
    }
}
