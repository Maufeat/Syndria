using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectListItem : MonoBehaviour
{
    public ItemPortrait portrait;
    public TMPro.TextMeshProUGUI heroName;
    public TMPro.TextMeshProUGUI powerLevel;
    public TMPro.TextMeshProUGUI heroLevel;

    public PlayerHero playerHero;

    public void Setup(PlayerHero hero)
    {
        playerHero = hero;
        //heroName.color = MathExt.getColorByRarity(hero.baseHeroData.BaseRarity);
        heroName.text = hero.template.name;
        heroLevel.text = $"Lv. { hero.level }";
        powerLevel.text = "Power: 910";
    }
}
