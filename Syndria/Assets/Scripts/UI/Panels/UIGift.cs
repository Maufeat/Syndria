using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGift : UIPanel
{
    public Button okBtn;
    
    void Start()
    {
        if(okBtn == null)
            okBtn = transform.Find("Image/Button").GetComponent<Button>();

        okBtn.onClick.AddListener(() => {
            Close();
        });
    }
}
