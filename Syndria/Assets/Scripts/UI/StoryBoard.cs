using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryBoard : UIPanel
{
    public Button closeBtn;

    public Button storyFightOne;
    public Button pvpFightBtn;

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });

        pvpFightBtn.onClick.AddListener(() =>
        {
            this.Close();
            UIManager.Instance.OpenPanel("MatchMaking");
        });

        storyFightOne.onClick.AddListener(() =>
        {
            ClientSend.StartFight(1);
        });
    }
}
