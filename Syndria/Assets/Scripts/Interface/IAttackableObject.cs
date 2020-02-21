using UnityEngine;

public interface IAttackableObject : ITileObject
{
    bool IsDead { get; }
    IStats Stats { get; }

    void ChangeModel(GameObject model);
    void TakeDamage(IAttackableObject attacker, float damage, bool isCrit);
    void Die(IAttackableObject killer);
}