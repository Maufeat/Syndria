using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderCamera : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 startPos;
    Vector3 hitPos;

    RectTransform canvasRect;
    RectTransform builderRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
        hitPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector3 deltaPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hitPos;
        Vector3 pos = startPos + deltaPos;
        pos.y = 0;
        pos.z = 5;
        transform.position = pos;
    }

    public void Start()
    {
        canvasRect = GameObject.Find("UIRoot").GetComponent<RectTransform>();
        builderRect = transform.GetComponent<RectTransform>();
    }

    public void Update()
    {
        var localPosition = builderRect.anchoredPosition;
        if (localPosition.x >= 0.0f)
        {
            localPosition.x = 0;
            builderRect.anchoredPosition = localPosition;
        }
        var x = canvasRect.rect.width + (transform.GetComponent<RectTransform>().rect.width * -1);
        if (localPosition.x <= x)
        {
            localPosition.x = x;
            builderRect.anchoredPosition = localPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
