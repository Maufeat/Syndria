using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public string prefabName;

    public bool shouldFade = false;
    public bool keepOnDispose = false;

    // Fade Options
    private float alpha = 0;
    public float FadeSpeed = 3;

    public virtual void Update()
    {
        if (shouldFade)
            Fade();
    }

    private void OnEnable()
    {
        if (shouldFade)
        {
            alpha = 0;
            if (GetComponent<CanvasGroup>() == null)
                gameObject.AddComponent<CanvasGroup>();
            GetComponent<CanvasGroup>().alpha = alpha;
        }
    }
    
    public virtual void Close()
    {
        if (UIManager.Instance.openPanels.Contains(this))
            UIManager.Instance.openPanels.Remove(this);
        DestroyImmediate(gameObject);
    }

    void Fade()
    {
        if (alpha >= 1 && GetComponent<CanvasGroup>() == null)
            return;
        alpha += Time.deltaTime * FadeSpeed;
        GetComponent<CanvasGroup>().alpha = alpha;
    }

}
