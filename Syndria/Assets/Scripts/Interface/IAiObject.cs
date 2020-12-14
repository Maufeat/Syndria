using System.Collections.Generic;
using UnityEngine;

public interface IAiObject : IAttackableObject
{
    //List<SpellData> spells { get; }

    void OnKill(IAttackableObject killed);
}