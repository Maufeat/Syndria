using Assets.Scripts.Battle;
using Assets.Scripts.Interface.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonation : Spell
{
    public float x_offset_fixed = -7.35f;
    public float y_offset_fixed = -2.45f;
    public float x_offset_multi = 1.85f;
    public float y_offset_multi = 1.5f;

    public override bool Cast(Vector2 location, List<Tile> targetCells)
    {
        foreach(var target in targetCells)
        {
            if(target.coordinate == location)
            {
                var x = new Vector3(location.x * x_offset_multi + x_offset_fixed, location.y * y_offset_multi + y_offset_fixed, 5);
                var go = Instantiate(spellData.anim, x, Quaternion.identity);
                go.transform.localScale = go.transform.localScale * 2;
            }
        }
        Debug.Log("Detonation::Cast");
        return true;
    }

    public override IEnumerator SpellAnimation()
    {
        throw new System.NotImplementedException();
    }
}
