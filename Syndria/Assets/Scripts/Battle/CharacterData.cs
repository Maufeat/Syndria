using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character
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
        //transform.position = new Vector3(location.x * mod3 + mod1, location.y * mod4 + mod2, 5);
        //GetComponent<SpriteRenderer>().sortingOrder = (99 - Convert.ToInt32(location.y));
    }

    public void Update()
    {
        /*var order = GetComponent<SpriteRenderer>().sortingOrder;
        if (order != (99 - Convert.ToInt32(location.y)))*/
    }
}
