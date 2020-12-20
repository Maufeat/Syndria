using UnityEngine;

public class AttackableObject : TileObject, IAttackableObject
{
    public bool IsDead { get; set; }

    public Stats Stats { get; set;  }
    
    public void ChangeModel(GameObject model)
    {
        throw new System.NotImplementedException();
    }

    public void Die(IAttackableObject killer)
    {
    }

    public void TakeDamage(IAttackableObject attacker, float damage, bool isCrit)
    {
        throw new System.NotImplementedException();
    }
}