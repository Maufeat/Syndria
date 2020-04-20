﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrepHeroItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject dragSprite;
    public Image disabledLayer;
    public PlayerHero hero;

    public bool canDrag = false;
    public bool showLevel = false;
    private bool disabled = false;

    void Start()
    {
        if(hero != null)
            SetupImages();
    }

    public void SetupImages()
    {
        transform.Find("Thumbnail").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Characters/{hero.baseHeroData.ID}/thumb");
        transform.Find("Rarity").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.baseHeroData.BaseRarity}");
        transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.baseHeroData.Type}");
        transform.Find("RankBorder").GetComponent<Image>().color = MathExt.getColorByRarity(hero.baseHeroData.BaseRarity);
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

        if (dragSprite == null)
        {
            dragSprite = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
            dragSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{hero.baseHeroData.ID}/sprite");
            dragSprite.GetComponent<SpriteRenderer>().flipX = true;
            dragSprite.transform.localScale -= new Vector3(0.5f, 0.5f);
            dragSprite.GetComponent<Animator>().SetBool("Grabbed", true);
            dragSprite.GetComponent<SpriteRenderer>().sortingOrder = 999;
        }
        
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.y -= 0.5f;
        mouseV3.z = 5;
        dragSprite.transform.position = mouseV3;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (disabled || !canDrag)
            return;
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.z = 5;
        if (dragSprite != null)
        {
            Destroy(dragSprite);
            dragSprite = null;
            BattleManager.Instance.SpawnCharacter(hero, mouseV3, true, this);
        }
    }
}
