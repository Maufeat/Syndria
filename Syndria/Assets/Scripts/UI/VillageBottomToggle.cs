using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageBottomToggle : MonoBehaviour
{
    public GameObject layoutGroup;

    public Button shopBtn;

    public TMPro.TextMeshProUGUI closeText;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (layoutGroup.activeSelf) { layoutGroup.SetActive(false); } else { layoutGroup.SetActive(true); }
        });

        shopBtn.onClick.AddListener(() =>
        {
            UIManager.instance.OpenPanel("UIShop");
        });
    }
    
    void Update()
    {
        if (layoutGroup.activeSelf)
        {
            closeText.transform.rotation = Quaternion.Euler(new Vector3(0,0,45));
        } else
        {
            closeText.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
}
