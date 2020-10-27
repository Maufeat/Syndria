using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Heroes : UIPanel
{
    public TextMeshProUGUI heroCounter;
    public GameObject itemContainer;
    public GameObject portraitPrefab;
    
    public PlayerHero selectedHero;

    // CurrentHero Selection Objects
    public TextMeshProUGUI selectedHeroName;
    public TextMeshProUGUI selectedHeroLevel;
    public TextMeshProUGUI selectedHeroExpLbl;
    public Slider selectedHeroExpProgressBar;
    public Image selectedHeroType;
    public Image selectedHeroRarity;
    public Image selectedHeroThumb;
    public TextMeshProUGUI selectedHeroPower;
    public TextMeshProUGUI selectedHeroHP;
    public TextMeshProUGUI selectedHeroMovement;
    public TextMeshProUGUI selectedHeroDamage;
    public TextMeshProUGUI selectedHeroRange;

    public GameObject selectedHeroRenderHolder;
    public GameObject selectedHeroRender;

    public GameObject PREFAB_TEST;

    public Button closeBtn;

    void Start()
    {
        if (Client.Instance.me == null)
            return;

        foreach(var hero in Client.Instance.me.heroes)
        {
            if (selectedHero == null)
                SelectHero(hero);

            var go = Instantiate(portraitPrefab, itemContainer.transform);
            HeroSelectListItem go_listItem = go.GetComponent<HeroSelectListItem>();
            go_listItem.Setup(hero);
            go_listItem.portrait.SetupPortraitAsHero(hero);
            go_listItem.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectHero(hero);
            });
        }

        closeBtn.onClick.AddListener(() => {
            Close();
        });

        heroCounter.text = $"Heroes: { Client.Instance.me.heroes.Count } / 10";
    }

    public void SelectHero(PlayerHero hero)
    {
        selectedHero = hero;

        // Renderer
        if (selectedHeroRender != null)
            Destroy(selectedHeroRender.gameObject);

        selectedHeroRender = Instantiate(hero.baseHeroData.overwriteGameObject, selectedHeroRenderHolder.transform);
        SpriteRenderer cacheSpriteRenderer = selectedHeroRender.GetComponent<SpriteRenderer>();
        cacheSpriteRenderer.transform.localPosition = new Vector3(85f, -100f);
        cacheSpriteRenderer.transform.localScale = new Vector3(30, 30);
        cacheSpriteRenderer.sortingOrder = 99;

        // SelectedValues
        selectedHeroName.text = hero.baseHeroData.Name;
        selectedHeroName.color = MathExt.getColorByRarity(hero.baseHeroData.BaseRarity);
        selectedHeroRarity.sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.baseHeroData.BaseRarity}");        selectedHeroType.sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.baseHeroData.Type}");
        selectedHeroLevel.text = $"Lv. { hero.level }";
        //selectedHeroPower.text = $"Power: 9000";
        //Resources.Load<Sprite>($"Images/Rarity/{(int)_heroData.BaseRarity}");
    }
        
}
