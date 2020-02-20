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
    public bool disabled = false;

    void Start()
    {
        sprite = Instantiate(Resources.Load("Prefabs/CharacterPrefab")) as GameObject;
        sprite.GetComponent<SpriteRenderer>().flipX = true;
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{id}_sprite");
        sprite.transform.localScale -= new Vector3(0.5f, 0.5f);

        SetupImages();
        sprite.SetActive(false);
    }

    public void SetupImages()
    {
        transform.Find("Thumbnail").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Characters/{id}_thumb");
        disabledLayer = transform.Find("DisableTrigger").GetComponent<Image>();
    }

    public void Update()
    {
        if (disabled)
            disabledLayer.color = new Color(0,0,0,0.5f);
        else
            disabledLayer.color = new Color(0, 0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (disabled)
            return;
        sprite.SetActive(true);
        sprite.GetComponent<SpriteRenderer>().sortingOrder = 999;
        sprite.GetComponent<Animator>().SetBool("Grabbed", true);
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.y -= 0.5f;
        mouseV3.z = 5;
        sprite.transform.position =  mouseV3;
        try
        {
            GameObject.Find("DragHighlight").SetActive(false);
        }catch(Exception e)
        {

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (disabled)
            return;
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.z = 5;
        Map map = null;
        try
        {
            map = GameObject.Find("BattleField").GetComponent<Map>();
        } catch(Exception e)
        {

        }
        if(map == null)
            map = GameObject.Find("TutorialFight").GetComponent<Map>();
        map.SpawnCharacter(id, map.GetTileByV3(mouseV3), true, this);
        sprite.SetActive(false);
    }
}
