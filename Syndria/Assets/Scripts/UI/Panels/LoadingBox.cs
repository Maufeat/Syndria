using System.Collections;
using UnityEngine;

public class LoadingBox : UIPanel
{
    public TMPro.TextMeshProUGUI loadingDescription;
    public TMPro.TextMeshProUGUI loadingTxt;

    public void Start()
    {
        StartCoroutine(this.Pulse());
    }

    public void SetText(string txt)
    {
        loadingDescription.text = txt;
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