using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interface.Battle
{
    public interface ISpell
    {
        FieldHero caster { get; set; }
        int cooldown { get; set; }
        Vector2 location { get; set; }
        // TODO: SpellData 
        
        bool Cast(Vector2 location, FieldHero target);
        IEnumerator SpellAnimation();
    }
}
