using UnityEngine;

namespace TDLogic
{
    public class Character : MonoBehaviour , IDamagable
    {
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public string Name { get; protected set; }
        public float Speed { get; protected set; }
        public float JumpHeight { get; protected set; }

        private protected void HelloWorld(string name)
        {
            Debug.Log($"this is {name} Enemy");
        }

        public virtual void SetStats(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
        }

        public virtual void SetStats(int health, int damage, string name, float speed, float jumpHeight)
        {
            Health = health;
            Damage = damage;
            Name = name;
            Speed = speed;
            JumpHeight = jumpHeight;
        }

        private protected void Die(GameObject gameObject)
        {
            Debug.Log(gameObject.name + " died!");
            Destroy(gameObject);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log($"{Name} took {damage} damage! Current health: {Health}");
            if (Health <= 0)
            {
                Die(gameObject);
            };
        }
    }
}