using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Items/Add Item")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public ItemType Type;
    public Rarity BaseRarity;
    public bool IsTwoHanded;
    public int SellPrice;
    public Sprite Image;
    public GameObject GameObject;

    public int qty { get; set; }
}
