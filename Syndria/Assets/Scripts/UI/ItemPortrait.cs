using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemPortrait : MonoBehaviour
{
    // GameObjects
    public Image go_itemThumb;
    public Image go_itemRarity;
    public Image go_itemType;
    public Image go_itemBorder;
    public TMPro.TextMeshProUGUI go_itemQuantity;

    public HeroData heroData;
    public ItemData itemData;

    public Button itemAction;

    void Start()
    {
        go_itemThumb = transform.Find("Thumbnail").GetComponent<Image>();
        go_itemRarity = transform.Find("Rarity").GetComponent<Image>();
        go_itemType = transform.Find("Type").GetComponent<Image>();
        go_itemBorder = transform.Find("RankBorder").GetComponent<Image>();
        if(transform.Find("Quantity"))
            go_itemQuantity = transform.Find("Quantity").GetComponent<TMPro.TextMeshProUGUI>();

        if (heroData != null)
            SetupPortraitAsHero(heroData);
        if (itemData != null)
            SetupPortraitAsItem(itemData);
    }

    public void SetupPortraitAsHero(PlayerHero hero)
    {
        heroData = hero.baseHeroData;
        go_itemThumb.sprite = Resources.Load<Sprite>($"Characters/{hero.baseHeroData.ID}/thumb");
        go_itemRarity.sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.baseHeroData.BaseRarity}");
        go_itemType.sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.baseHeroData.Type}");
        go_itemBorder.color = MathExt.getColorByRarity(hero.baseHeroData.BaseRarity);
    }

    public void SetupPortraitAsHero(HeroData hero)
    {
        heroData = hero;
        go_itemThumb.sprite = Resources.Load<Sprite>($"Characters/{hero.ID}/thumb");
        go_itemRarity.sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.BaseRarity}");
        go_itemType.sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.Type}");
        go_itemBorder.color = MathExt.getColorByRarity(hero.BaseRarity);
    }

    public void SetupPortraitAsItem(ItemData item)
    {
        itemData = item;
        go_itemQuantity.text = item.qty.ToString();
        go_itemThumb.sprite = item.Image;
        go_itemBorder.color = MathExt.getColorByRarity(item.BaseRarity);
        go_itemQuantity.gameObject.SetActive(true);
        go_itemRarity.gameObject.SetActive(false);
        go_itemType.gameObject.SetActive(false);
    }

    public void OnClick(UnityAction action)
    {
        itemAction.onClick.RemoveAllListeners();
        itemAction.onClick.AddListener(action);
    }
}
