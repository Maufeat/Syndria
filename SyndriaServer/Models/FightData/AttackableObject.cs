using SyndriaServer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer.Models.FightData
{
    public class AttackableObject : TileObject, IAttackableObject
    {
        public bool IsDead { get; set; }

        public IStats Stats { get; set; }

        public void Die(IAttackableObject killer)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(IAttackableObject attacker, float damage, bool isCrit)
        {
            throw new NotImplementedException();
        }
    }
}
