using System;
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
    public bool disabled = false;

    void Start()
    {
        if(hero != null)
            SetupImages();
    }

    public void SetupImages()
    {
        transform.Find("Thumbnail").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Characters/{hero.template.id}/thumb");
        transform.Find("Rarity").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.template.rarity}");
        transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.template.type}");
        transform.Find("RankBorder").GetComponent<Image>().color = MathExt.getColorByRarity(hero.template.rarity);
        if (!showLevel)
            transform.Find("Level").gameObject.SetActive(false);
        disabledLayer = transform.Find("DisableTrigger").GetComponent<Image>();
    }

    public void SetupImagesByHeroData(HeroTemplate _heroData)
    {
        transform.Find("Thumbnail").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Characters/{_heroData.id}/thumb");
        transform.Find("Rarity").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)_heroData.rarity}");
        transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Type/{(int)_heroData.type}");
        transform.Find("RankBorder").GetComponent<Image>().color = MathExt.getColorByRarity(_heroData.rarity);
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
            dragSprite = Instantiate(hero.template.overwriteGameObject);
            dragSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{hero.template.id}/sprite");
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
            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.SendSetPrepHero(hero, mouseV3);
            }
            else
            {
                Client.Instance.OnHeroListDrag(hero, mouseV3);
            }
        }
    }

    void Update()
    {
        if (BattleManager.Instance)
        {
            if (!BattleManager.Instance.started)
            {
                SetDisabled(true);
            }
        }
    }
}
