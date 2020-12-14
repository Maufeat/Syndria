using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PleaseUpdate : UIPanel
{
    public Button dlBtn;

    void Start()
    {
        dlBtn.onClick.AddListener(() =>
        {
            NetworkManager.Instance.Disconnect();
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.pockie.mobile");
        });
    }
}
