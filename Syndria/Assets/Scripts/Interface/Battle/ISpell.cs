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
        int cooldown { get; set; }
        Vector2 location { get; set; }
        SpellData spellData { get; set; }
        
        bool Cast(Vector2 location, List<Tile> target);
        IEnumerator SpellAnimation();
    }
}
