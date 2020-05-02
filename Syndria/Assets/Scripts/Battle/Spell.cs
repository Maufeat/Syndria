using Assets.Scripts.Interface.Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class Spell : ISpell
    {
        public FieldHero caster { get; set; }
        public int cooldown { get; set; }
        public Vector2 location { get; set; }

        public Spell(FieldHero _caster)
        {
            caster = _caster;
        }

        // Target can be null, because not every spell is targeted
        public virtual bool Cast(Vector2 location, FieldHero target = null)
        {
            Debug.Log("Scripts.Battle.Spell::Cast");
            return true;
        }

        public IEnumerator SpellAnimation()
        {
            Debug.Log("Scripts.Battle.Spell::SpellAnimation");
            yield return new WaitForSeconds(0.2f);
        }
    }
}
