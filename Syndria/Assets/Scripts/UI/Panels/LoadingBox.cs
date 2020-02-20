using System.Collections;
using UnityEngine;

public class LoadingBox : UIPanel
{
    private TMPro.TextMeshProUGUI loadingDescription;
    private TMPro.TextMeshProUGUI loadingTxt;
    public string context = "";

    public void Start()
    {
        loadingDescription = GameObject.Find("LoadingDescription").GetComponent<TMPro.TextMeshProUGUI>();
        loadingTxt = GameObject.Find("LoadingText").GetComponent<TMPro.TextMeshProUGUI>();
        loadingDescription.text = context;

        UIManager.instance.currentLoadingBox = this;

        StartCoroutine(this.Pulse());
    }

    public void SetText(string txt)
    {
        if (UIManager.instance.currentLoadingBox != null)
            loadingDescription.text = txt;
        else
            context = txt;
    }

    IEnumerator Pulse()
    {
        while (true)
        {
            while (loadingTxt.fontSize != 45)
            {
                loadingTxt.fontSize = Mathf.MoveTowards(loadingTxt.fontSize, 45, 0.03f);
                yield return new WaitForEndOfFrame();
            }

            while (loadingTxt.fontSize != 40)
            {
                loadingTxt.fontSize = Mathf.MoveTowards(loadingTxt.fontSize, 40, 0.03f);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}