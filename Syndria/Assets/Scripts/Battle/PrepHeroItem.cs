using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrepHeroItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int id = 1;

    public GameObject sprite;
    public Image disabledLayer;
    public HeroData hero;

    public bool canDrag = false;
    public bool showLevel = false;
    private bool disabled = false;

    void Start()
    {
        hero = Resources.Load<HeroData>($"Characters/{id}/data");
        sprite = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
        sprite.GetComponent<SpriteRenderer>().flipX = true;
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{id}/sprite");
        sprite.transform.localScale -= new Vector3(0.5f, 0.5f);

        SetupImages();

        sprite.SetActive(false);
    }

    public void SetupImages()
    {
        transform.Find("Thumbnail").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Characters/{id}/thumb");
        transform.Find("Rarity").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.BaseRarity}");
        transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.Type}");
        transform.Find("RankBorder").GetComponent<Image>().color = MathExt.getColorByRarity(hero.BaseRarity);
        if (!showLevel)
            transform.Find("Level").gameObject.SetActive(false);
        disabledLayer = transform.Find("DisableTrigger").GetComponent<Image>();
    }
    
    public void SetDisabled(bool _disabled)
    {
        disabled = _disabled;
        if (disabled)
            disabledLayer.color = new Color(0, 0, 0, 0.5f);
        else
            disabledLayer.color = new Color(0, 0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (disabled || !canDrag)
            return;
        sprite.SetActive(true);
        sprite.GetComponent<SpriteRenderer>().sortingOrder = 999;
        sprite.GetComponent<Animator>().SetBool("Grabbed", true);
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.y -= 0.5f;
        mouseV3.z = 5;
        sprite.transform.position = mouseV3;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (disabled || !canDrag)
            return;
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.z = 5;
        BattleManager.instance.SpawnCharacter(hero, mouseV3, true, this);
        sprite.SetActive(false);
    }
}
