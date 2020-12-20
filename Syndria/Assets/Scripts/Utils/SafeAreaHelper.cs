using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaHelper : MonoBehaviour
{
    private Canvas canvas;
    private Rect lastSafeArea = Rect.zero;

    private void Awake()
    {
        canvas = GameObject.Find("UIRoot").GetComponent<Canvas>();
    }

    private void Update()
    {
        if (lastSafeArea != Screen.safeArea)
        {
            lastSafeArea = Screen.safeArea;
            ApplySafeArea();
        }
    }

    void Start()
    {
        lastSafeArea = Screen.safeArea;
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        GetComponent<RectTransform>().anchorMin = anchorMin;
        GetComponent<RectTransform>().anchorMax = anchorMax;
    }

}