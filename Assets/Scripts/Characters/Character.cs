using UnityEngine;

namespace TDLogic
{
    /// <summary>
    /// a base class for characters with common properties
    /// </summary>
    public class Character : MonoBehaviour, IDamagable, IHealth
    {
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public string Name { get; protected set; }
        public float Speed { get; protected set; }
        public float JumpHeight { get; protected set; }

        public int CurrentHealth => Health;


        //presents the name of the character
        private protected void HelloWorld(string name)
        {
            Debug.Log($"this is {name} Character");
        }

        public virtual void SetStats(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
        }

        public virtual void SetStats(int health, int damage, string name, float speed, float jumpHeight)
        {
            SetStats(health, damage, name);
            Speed = speed;
            JumpHeight = jumpHeight;
        }


        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            Debug.Log($"{name} took {amount} damage! Current health: {Health}");
            if (Health <= 0)
            {
                Die(gameObject);
            }
        }

        public virtual void Heal(int amount)
        {
            Health += amount;
            Debug.Log($"{name} healed {amount}. Current health: {Health}");
        }
        private protected void Die(GameObject gameObject)
        {
            Debug.Log(name + " died!");
            Destroy(gameObject);
        }
    }
}