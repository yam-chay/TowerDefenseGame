using Unity.IO.LowLevel.Unsafe;
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

        private protected void Init(CharacterData characterData)
        {
            Name = characterData.characterName;
            Health = characterData.health;
            Damage = characterData.damage;
            Range = characterData.range;
        }

        public override string ToString()
        {
            return $"this is {name} Character";
        }


        private protected void HelloWorld(string name)
        {
            Debug.Log($"this is {name} Character");
        }

        public virtual void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, Range);
            UtilsClass.Attack(hitList, Damage, gameObject);
        }

        public virtual void TakeDamage(int damage, Transform attacker)
        {
            Health -= damage;
            Debug.Log($"{name} took {damage} damage! Current health: {Health}");
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

        public virtual void Die(GameObject gameObject)
        {
            Debug.Log(name + " died!");
            Destroy(gameObject, 1f);
        }
    }
}