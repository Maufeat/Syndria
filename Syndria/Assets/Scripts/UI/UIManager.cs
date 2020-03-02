using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject uiLogin;

    public UIPanel currentMain;

    public List<UIPanel> openPanels = new List<UIPanel>();

    public LoadingBox currentLoadingBox;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    public void OpenPanel(string name, bool isMain = false)
    {
        var panel = Resources.Load("Prefabs/UI/" + name) as GameObject;
        var obj = Instantiate(panel, gameObject.transform);
        obj.name = panel.name;
        if (isMain)
        {
            if (currentMain != null) currentMain.Close();
            currentMain = obj.GetComponent<UIPanel>();
        }
    }

    public LoadingBox OpenLoadingBox(string txt)
    {
        if(currentLoadingBox != null)
        {
            currentLoadingBox.SetText(txt);
            return currentLoadingBox;
        } else
        {
            var panel = Resources.Load("Prefabs/UI/Loading") as GameObject;
            panel.GetComponent<LoadingBox>().SetText(txt);
            var obj = Instantiate(panel, gameObject.transform);
            return null;
        }
    }

    public void OpenMsgBox(string txt)
    {
        var panel = Resources.Load("Prefabs/UI/MessageBox") as GameObject;
        panel.GetComponent<MessageBox>().SetText(txt);
        panel.GetComponent<MessageBox>().SetupBtnOne("OK");
        var obj = Instantiate(panel, gameObject.transform);
    }

    public void CloseLoadingBox()
    {
        if (currentLoadingBox != null)
        {
            currentLoadingBox.Close();
            currentLoadingBox = null;
        }
    }

    public void CloseAllPanel(bool forceAll = false)
    {
        foreach(var panel in openPanels)
        {
            if (panel.keepOnDispose && !forceAll) continue;
            panel.Close();
            openPanels.Remove(panel);
        }
    }
    
    public void ClosePanel(UIPanel panel)
    {
        openPanels.Remove(panel);
        panel.Close();
    }

}