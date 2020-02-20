using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroEditor : EditorWindow
{
    string heroName;
    CharType heroType;
    Rarity baseRarity;
    Sprite sprite;
    Sprite inventorySprite;
    List<SpellData> spells = new List<SpellData>();

    [MenuItem("Pocket Ninja/Ninja editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(HeroEditor));
    }


    void OnGUI()
    {
        heroName = EditorGUILayout.TextField("Name:", heroName);
        heroType = (CharType)EditorGUILayout.EnumPopup("Type:", heroType);
        baseRarity = (Rarity)EditorGUILayout.EnumPopup("Base Rarity: ", baseRarity);
        sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", sprite, typeof(Sprite), false);
        inventorySprite = (Sprite)EditorGUILayout.ObjectField("Inventory Sprite", inventorySprite, typeof(Sprite), false);
    }
}
