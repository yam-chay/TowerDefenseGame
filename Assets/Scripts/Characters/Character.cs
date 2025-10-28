using UnityEngine;

namespace KingdomScratch
{
   /// <summary>
   /// Base class for all character entities. 
   /// Handles common attributes and combat behavior.
   /// </summary>
    public class Character : MonoBehaviour, IDamagable
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public float Range { get; protected set; }


        /// <summary>
        /// Initializes the character's core stats from CharacterData.
        /// </summary>
        /// <param name="characterData">The data asset containing base stats.</param>
        private protected void Init(CharacterData characterData)
        {
            Name = characterData.characterName;
            Health = characterData.health;
            Damage = characterData.damage;
            Range = characterData.range;
        }

        /// <summary>
        /// A hello method so each character says hi on console when he awakes up
        /// </summary>
        public override string ToString()
        {
            return $"this is {name} Character";
        }

        /// <summary>
        /// Executes an attack on nearby valid targets.
        /// </summary>
        private protected void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, Range);
            UtilsClass.Attack(hitList, Damage, gameObject);
        }

        /// <summary>
        /// Applies damage and triggers default destroy death when health drops below zero.
        /// </summary>
        public virtual void TakeDamage(int damage, Transform attacker)
        {
            Health -= damage;
            Debug.Log($"{name} took {damage} damage! Current health: {Health}");
        }

        /// <summary>
        /// Restores health by a specified amount.
        /// </summary>
        private protected void Heal(int amount)
        {
            Health += amount;
            Debug.Log($"{name} healed {amount}. Current health: {Health}");
        }
    }
}