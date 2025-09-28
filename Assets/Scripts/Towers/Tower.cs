using UnityEngine;

namespace TDLogic
{
    public class Tower : MonoBehaviour , IDamagable
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public float Radius { get; private set; }

        public void Heal(int amount)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Init(TowerData towerData)
        {
            Name = towerData.name;
            Health = towerData.health;
            Damage = towerData.damage;
            Radius = towerData.radius;
        }

        public void TakeDamage(int damage, Transform attacker)
        {
            Health -= damage;
            Debug.Log($"{name} took {damage} damage! Current health: {Health}");
            if (Health <= 0)
            {
                Die(gameObject);
            }
        }

        private protected virtual void HelloWorld(string name)
        {
            Debug.Log($"this is {name} Tower");
        }

        private protected void Die(GameObject gameObject)
        {
            Debug.Log(name + " died!");
            Destroy(gameObject, 0.1f);
        }

    }
}
