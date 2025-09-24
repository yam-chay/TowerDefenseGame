using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TDLogic
{
    public static class UtilsClass
    {
        /// <summary>
        /// provide a list of all colliders in radius from position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Collider2D[] GetTargetsInRadius(Vector2 position, float radius)
        {
            return Physics2D.OverlapCircleAll(position, radius);
        }

        /// <summary>
        /// Apply Damage to any IDamagable object in a list of collider {exclude caller}
        /// </summary>
        /// <param name="attackList"></param>
        /// <param name="damage"></param>
        /// <param name="attacker"></param>
        public static void Attack(Collider2D[] attackList, int damage, GameObject attacker)
        {
            foreach (Collider2D collider in attackList)
            {
                if (collider.TryGetComponent<IDamagable>(out var damagable))
                {
                    if(collider.gameObject.layer != attacker.layer)
                    {
                        damagable.TakeDamage(damage);
                    }
                }
            }
        }

        public static void Interact(Collider2D[] interactList, Transform interactor)
        {
            foreach (Collider2D collider in interactList)
            {
                if (collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Interact(interactor);
                }
            }
        }
    }
}