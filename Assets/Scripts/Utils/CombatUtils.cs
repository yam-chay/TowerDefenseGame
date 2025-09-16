using System.Collections;
using TDLogic;
using UnityEngine;

public static class CombatUtils
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
    /// <param name="caller"></param>
    public static void Attack(Collider2D[] attackList, int damage, GameObject caller)
    {
        foreach (Collider2D collider in attackList)
        {
            if (collider.gameObject != caller)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                }
            }
            else
            {

            }

        }
    }
}
