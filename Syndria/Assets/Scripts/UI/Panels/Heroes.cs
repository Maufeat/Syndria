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
    public GameObject selectedHeroSpells;
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

        selectedHeroRender = Instantiate(hero.template.overwriteGameObject, selectedHeroRenderHolder.transform);
        SpriteRenderer cacheSpriteRenderer = selectedHeroRender.GetComponent<SpriteRenderer>();
        cacheSpriteRenderer.transform.localPosition = new Vector3(0, -100f);
        cacheSpriteRenderer.transform.localScale = new Vector3(30, 30);
        cacheSpriteRenderer.sortingOrder = 99;

        // SelectedValues
        selectedHeroName.text = hero.template.name;
        selectedHeroName.color = MathExt.getColorByRarity(hero.template.rarity);
        selectedHeroRarity.sprite = Resources.Load<Sprite>($"Images/Rarity/{(int)hero.template.rarity}");        selectedHeroType.sprite = Resources.Load<Sprite>($"Images/Type/{(int)hero.template.type}");
        selectedHeroLevel.text = $"Lv. { hero.level }";
        //selectedHeroPower.text = $"Power: 9000";
        int i = 0;
        foreach(Transform spells in selectedHeroSpells.transform)
        {
            if (hero.spellData[i] != null)
            {
                spells.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.LogError("clicked Spell");
                });
                spells.Find("Mask/Image").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                spells.Find("Mask/Image").GetComponent<Image>().sprite = hero.spellData[i].sprite;
            }

            i++;
        }
        //Resources.Load<Sprite>($"Images/Rarity/{(int)_heroData.BaseRarity}");
    }
        
}
