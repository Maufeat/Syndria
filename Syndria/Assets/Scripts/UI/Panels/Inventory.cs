using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : UIPanel
{
    public GameObject inventoryScrolLView;
    public GameObject portraitPrefab;

    public Button closeBtn;

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close();
        });

        foreach (var item in Client.Instance.me.items)
        {
            var go = Instantiate(portraitPrefab, inventoryScrolLView.transform);
            ItemPortrait go_listItem = go.GetComponent<ItemPortrait>();

            var itemGO = new GameObject();
            itemGO.transform.SetParent(go.transform);
            var gotemp = Instantiate(item.GameObject, go.transform);
            var gotempItemHolder = gotemp.GetComponent<ItemHolder>();
            gotempItemHolder.item.itemData = item;
            go_listItem.itemData = item;
            go_listItem.SetupPortraitAsItem(item);
            go_listItem.GetComponent<Button>().onClick.AddListener(() =>
            {
                var obj = UIManager.Instance.OpenPanel("UIItemDetails");
                var xi = obj.GetComponent<ItemDetails>();
                xi.itemData = item;
                Debug.Log("Clicked on: " + item.Name);
            });
        }    
    }
}
