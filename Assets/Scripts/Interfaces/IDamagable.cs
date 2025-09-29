using UnityEngine;

namespace TDLogic
{
    public interface IDamagable
    {
        int Health { get; }
        int Damage { get; }
        void TakeDamage(int damage, Transform attacker);
        void Heal(int amount);
    }
}
