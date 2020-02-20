using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CharType
{
    Rock,
    Paper,
    Scissor
}

public enum Rarity
{
    R,
    SR,
    SSR
}

public class CharacterData : MonoBehaviour, IDragHandler, IDropHandler
{
    public int ID;
    public string Name;
    public CharType Type;
    public Rarity Rarity;
    public Stats Stats;
    public GameObject BattleSprite;
    public Sprite InventoryIcon;

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Character");
        GetComponent<SpriteRenderer>().sortingOrder = 999;
        GetComponent<Animator>().SetBool("Grabbed", true);
        var mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseV3.y -= 0.5f;
        mouseV3.z = 5;
        transform.position = mouseV3;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropping Character");
    }
}

public class Character : CharacterData
{
    public string GUID;
    public int Level;
    public int Experience;
    public List<SpellData> Spells;

    public Vector2 location = new Vector2(0, 0);

    public float mod1 = -7.35f;
    public float mod2 = -2.45f;
    public float mod3 = 1.85f;
    public float mod4 = 1.5f;


    public void SetPosition(float x, float y)
    {
        location.Set(x, y);
        transform.position = new Vector3(location.x * mod3 + mod1, location.y * mod4 + mod2, 5);
        GetComponent<SpriteRenderer>().sortingOrder = (99 - Convert.ToInt32(location.y));
    }

    public void Update()
    {
        /*var order = GetComponent<SpriteRenderer>().sortingOrder;
        if (order != (99 - Convert.ToInt32(location.y)))*/
    }
}
