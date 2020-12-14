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

    public HeroTemplate heroData;
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
        heroData = hero.template;
        go_itemThumb.sprite = Resources.Load<Sprite>($"Characters/{hero.template.id}/thumb");
        go_itemRarity.sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.template.rarity}");
        go_itemType.sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.template.type}");
        go_itemBorder.color = MathExt.getColorByRarity(hero.template.rarity);
    }

    public void SetupPortraitAsHero(HeroTemplate hero)
    {
        heroData = hero;
        go_itemThumb.sprite = Resources.Load<Sprite>($"Characters/{hero.id}/thumb");
        go_itemRarity.sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.rarity}");
        go_itemType.sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.type}");
        go_itemBorder.color = MathExt.getColorByRarity(hero.rarity);
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
