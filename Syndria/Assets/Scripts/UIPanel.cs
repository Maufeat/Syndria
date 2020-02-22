using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public bool shouldFade = false;

    public bool keepOnDispose = false;

    // Fade Options
    private float alpha = 0;
    public float FadeSpeed = 2;

    public void Update()
    {
        if (shouldFade)
            FadeAll();
    }

    private void OnEnable()
    {
        if (shouldFade)
        {
            alpha = 0;

            foreach (TMPro.TextMeshProUGUI text in gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
                text.color = new Color(text.color.r, text.color.g, text.color.g, 0);

            foreach (Image renderer in gameObject.GetComponentsInChildren<Image>())
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.g, 0);
        }
    }
    
    public virtual void Close()
    {
        Destroy(gameObject);
    }

    void FadeAll()
    {
        if (alpha >= 1)
            return;
        alpha += Time.deltaTime * FadeSpeed;

        foreach (TMPro.TextMeshProUGUI text in gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            text.color = new Color(text.color.r, text.color.g, text.color.g, alpha);
        }

        foreach (Image renderer in gameObject.GetComponentsInChildren<Image>())
        {
            if (renderer.gameObject.name == "Backdrop" && alpha >= 0.5f)
                continue;

            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.g, alpha);
        }
    }

}
