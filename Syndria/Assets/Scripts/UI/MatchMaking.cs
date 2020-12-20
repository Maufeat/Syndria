using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchMaking : UIPanel
{
    public Button closeBtn;

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });
    }
}
