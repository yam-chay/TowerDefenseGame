using UnityEngine;

namespace TDLogic
{
    public class Tower : MonoBehaviour 
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public float Radius { get; private set; }

        public virtual void SetStats(string name, int health, int damage , float radius)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Radius = radius;
        }

        private protected virtual void HelloWorld(string name)
        {
            Debug.Log($"this is {name} Tower");
        }

    }
}
