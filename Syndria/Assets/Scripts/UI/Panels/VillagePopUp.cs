using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillagePopUp : UIPanel
{
    public BuildingData data;

    public Image buildingImage;
    public TMPro.TextMeshProUGUI buildingName;

    public Button goToBtn;
    public Button upgradeBtn;
    public Button closeBtn;

    void Start()
    {
        if(data != null)
        {
            buildingImage.sprite = data.Sprite;
            buildingName.text = data.Name;
        }
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });
    }
}
