using Assets.Scripts.Interface;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    public ItemData itemData { get; set; }

    public virtual bool OnUse(int qty = 1)
    {
        return false;
    }
}