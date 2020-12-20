using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectServer : UIPanel
{
    public Login login;

    public GameObject serverListContainer;
    public GameObject serverListItem;

    // Start is called before the first frame update
    void Start()
    {
        if (login == null)
            login = GameObject.Find("UILogin").GetComponent<Login>();

        foreach(var server in login.serverList)
        {
            var o = Instantiate(serverListItem, serverListContainer.transform);
            o.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = server.Value["name"].ToString();
            o.GetComponent<Button>().onClick.AddListener(() => {
                login.ChangeSelectedServer(Convert.ToInt32(server.Value["id"]));
                Close();
            });
        }
    }
}
