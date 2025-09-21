using UnityEngine;

namespace TDLogic
{
    /// <summary>
    /// a base class for characters with common properties
    /// </summary>
    public class Character : MonoBehaviour, IDamagable
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get;  protected set; }
        public float Range { get; protected set; }

        //presents the name of the character

        public virtual void Init(CharacterData characterData)
        {
            Name = characterData.characterName;
            Health = characterData.health;
            Damage = characterData.damage;
            Range = characterData.range;
        }

        private protected void HelloWorld(string name)
        {
            Debug.Log($"this is {name} Character");
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