using UnityEngine;

public class AttackableObject : TileObject, IAttackableObject
{
    public bool IsDead { get; }

    public IStats Stats { get; }
    
    public void ChangeModel(GameObject model)
    {
        throw new System.NotImplementedException();
    }

    public void Die(IAttackableObject killer)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(IAttackableObject attacker, float damage, bool isCrit)
    {
        throw new System.NotImplementedException();
    }
}