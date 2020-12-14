using Assets.Scripts.Battle;
using Assets.Scripts.Interface.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Spell
{
    public override bool Cast(Vector2 location, List<Tile> targetCells)
    {
        foreach(var target in targetCells)
        {
            if(target.objectOnTile is Hero)
            {
                Hero targetHero = target.objectOnTile as Hero;
                FieldHero fieldHero = BattleManager.Instance.battleMap.GetFieldHeroById(targetHero.ID);
                var go = Instantiate(spellData.anim, fieldHero.transform.position, Quaternion.identity);
            }
        }
        Debug.Log("Heal::Cast");
        return true;
    }

    public override IEnumerator SpellAnimation()
    {
        throw new System.NotImplementedException();
    }
}
