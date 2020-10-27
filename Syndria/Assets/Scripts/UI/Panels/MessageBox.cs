using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageBox : UIPanel
{
    private string okButtonText = "Ok";
    private Button okButton;
    public UnityAction actionOne;

    private string cancelButtonText = "Cancel";
    private Button cancelButton;
    private UnityAction actionTwo;

    private TMPro.TextMeshProUGUI boxTxt;
    
    public string context = "";

    public void Start()
    {
        okButton = GameObject.Find("OkBtn").GetComponent<Button>();
        okButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = okButtonText;
        if(actionOne != null)
            okButton.GetComponent<Button>().onClick.AddListener(actionOne);
        else
            okButton.GetComponent<Button>().onClick.AddListener(delegate { UIManager.Instance.ClosePanel(this); });
        try
        {
            cancelButton = GameObject.Find("CancelBtn").GetComponent<Button>();
            cancelButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = cancelButtonText;
            if (actionTwo != null)
                cancelButton.GetComponent<Button>().onClick.AddListener(actionTwo);
            else
                cancelButton.GetComponent<Button>().onClick.AddListener(delegate { UIManager.Instance.ClosePanel(this); });
        } catch(Exception e) { Debug.Log(e.Message); }
        
        boxTxt = GameObject.Find("Context").GetComponent<TMPro.TextMeshProUGUI>();
        boxTxt.text = context;

        UIManager.Instance.OpenPanel(this);

        if (actionTwo == null && cancelButton != null)
        {
            cancelButton.gameObject.SetActive(false);
        }
    }

    public void SetText(string txt)
    {
        context = txt;
    }

    public void SetupBtnOne(string text, UnityAction action = null)
    {
        okButtonText = text;
        actionOne = action;
    }

    public void SetupCancelOne(string text, UnityAction action = null)
    {
        cancelButtonText = text;
        actionTwo = action;
    }

    public new void Update()
    {
        base.Update();
    }
}
