using UnityEngine;

namespace TDLogic
{
    public class Enemy : Character
    {

        private void Start()
        {
            SetStats(50, 5, "Enemy");
        }

        public void TakeDamage(int damage)
        {
            void Attack(GameObject target)
            {
                IDamagable damagable = target.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(Damage);
                }
            }

        }
    }
}