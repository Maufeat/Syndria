﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject mainLayer;
    public GameObject chatLayer;
    public GameObject popLayer;

    public UIPanel currentMain;

    public List<UIPanel> openPanels = new List<UIPanel>();

    public GameObject currentLoadingBox;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
    }

    public void OpenPanel(UIPanel panel)
    {
        openPanels.Add(panel);

    }

    public GameObject OpenPanel(string name, bool isMain = false)
    {
        var panel = Resources.Load("Prefabs/UI/" + name) as GameObject;
        var obj = Instantiate(panel, mainLayer.transform);
        obj.name = panel.name;
        if (isMain)
        {
            if (currentMain != null) currentMain.Close();
            currentMain = obj.GetComponent<UIPanel>();
        }

        if (obj.GetComponent<UIPanel>() != null)
        {
            openPanels.Add(obj.GetComponent<UIPanel>());
        }

        return obj;
    }

    public void OpenLoadingBox(string txt)
    {
        if(currentLoadingBox != null)
        {
            currentLoadingBox.GetComponent<LoadingBox>().SetText(txt);
        } else
        {
            var panel = Resources.Load("Prefabs/UI/Loading") as GameObject;
            currentLoadingBox = Instantiate(panel, popLayer.transform);
            currentLoadingBox.GetComponent<LoadingBox>().SetText(txt);
        }
    }

    public void OpenMsgBox(string txt)
    {
        var panel = Resources.Load("Prefabs/UI/MessageBox") as GameObject;
        panel.GetComponent<MessageBox>().SetText(txt);
        panel.GetComponent<MessageBox>().SetupBtnOne("OK");
        var obj = Instantiate(panel, popLayer.transform);
    }

    public void CloseLoadingBox()
    {
        if (currentLoadingBox != null)
        {
            currentLoadingBox.GetComponent<LoadingBox>().Close();
            currentLoadingBox = null;
        }
    }

    public void CloseAllPanel(bool forceAll = false)
    {
        foreach(var panel in openPanels.ToArray())
        {
            if(!forceAll)
                if (panel.keepOnDispose)
                    continue;

            if (!(panel is MessageBox))
            {
                panel.Close();
                openPanels.Remove(panel);
            }
        }
    }
    
    public void ClosePanel(UIPanel panel)
    {
        openPanels.Remove(panel);
        panel.Close();
    }

}