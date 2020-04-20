using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Button>() != null)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("Sounds/SFX/btn_click"));
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
