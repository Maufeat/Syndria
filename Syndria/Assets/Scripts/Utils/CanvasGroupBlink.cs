using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupBlink : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool _switch;

    public float max = 0.5f;
    public float min = 0.1f;
    public float speed = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if((canvasGroup.alpha > max && _switch) || (canvasGroup.alpha < min && !_switch))
            _switch = !_switch;
            
        if (_switch)
            canvasGroup.alpha += Time.deltaTime * speed;
        else
            canvasGroup.alpha -= Time.deltaTime * speed;
    }
}
