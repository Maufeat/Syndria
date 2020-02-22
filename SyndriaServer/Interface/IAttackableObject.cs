namespace SyndriaServer.Interface
{
    public interface IAttackableObject : ITileObject
    {
        bool IsDead { get; }
        IStats Stats { get; }
        
        void TakeDamage(IAttackableObject attacker, float damage, bool isCrit);
        void Die(IAttackableObject killer);
    }
}
