using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : UIPanel
{
    public Button closeBtn;
    public Button useBtn;

    public ItemPortrait itemPortrait;

    public TMPro.TextMeshProUGUI itemName;
    public TMPro.TextMeshProUGUI itemDescription;

    public HeroTemplate heroData;
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
        useBtn.onClick.AddListener(() =>
        {
            var goScript = Instantiate(itemData.GameObject, gameObject.transform);
            goScript.GetComponent<ItemHolder>().item.OnUse(1);
        });
        useBtn.gameObject.SetActive(true);
    }

    public void SetupAsHero(HeroTemplate _heroData)
    {
        heroData = _heroData;
        itemPortrait.SetupPortraitAsHero(heroData);
        itemName.text = heroData.name;
        itemDescription.text = heroData.description;
        useBtn.gameObject.SetActive(false);
    }
}
