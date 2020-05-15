using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : UIPanel
{
    public Button closeBtn;

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });  
    }

    void Update()
    {
        
    }
}
