using System.Collections.Generic;
using UnityEngine;

public interface IAiObject : IAttackableObject
{
    HeroData heroData { get; }
    List<SpellData> spells { get; }

    void OnKill(IAttackableObject killed);
}