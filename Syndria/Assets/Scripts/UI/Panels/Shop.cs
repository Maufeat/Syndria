using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : UIPanel
{
    public Button closeBtn;
    public Button adBtn;

    void Start()
    {
        adBtn.onClick.AddListener(() =>
        {
            Client.Instance.ShowRewardedVideo();
        });
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });  
    }
}
