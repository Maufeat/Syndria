using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryBoard : UIPanel
{
    public Button closeBtn;

    public Button storyFightOne;

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });

        storyFightOne.onClick.AddListener(() =>
        {
            ClientSend.StartFight(1);
        });
    }
}
