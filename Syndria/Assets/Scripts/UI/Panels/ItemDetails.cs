using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : UIPanel
{
    public Button closeBtn;

    public ItemPortrait itemPortrait;

    public TMPro.TextMeshProUGUI itemName;
    public TMPro.TextMeshProUGUI itemDescription;

    public HeroData heroData;
    public ItemData itemData;
    
    public void Start()
    {
        if (heroData != null)
            SetupAsHero(heroData);
        if (itemData != null)
            SetupAsItem(itemData);
        
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });

    }

    public void SetupAsItem(ItemData _itemData)
    {
        itemData = _itemData;
        itemPortrait.SetupPortraitAsItem(itemData);
        itemName.text = itemData.Name;
        itemDescription.text = itemData.Description;
    }

    public void SetupAsHero(HeroData _heroData)
    {
        heroData = _heroData;
        itemPortrait.SetupPortraitAsHero(heroData);
        itemName.text = heroData.Name;
        itemDescription.text = heroData.Description;
    }
}
