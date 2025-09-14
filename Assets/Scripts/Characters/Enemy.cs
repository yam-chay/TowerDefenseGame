using System.Collections;
using UnityEngine;

namespace TDLogic
{
    public class Enemy : Character
    {
        private float attackradius = 1;
        [SerializeField] private GameObject hitEffect;

        private void Start()
        {
            SetStats(50, 5, "Enemy");
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        public Collider2D[] OverlapRadius()
        {
            Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, attackradius);

            foreach (Collider2D hit in hitList)
            {
                Debug.Log($"{hit} is in range");
            }

            return hitList;
        }

        private void Attack(Collider2D[] attackList, int damage)
        {
            foreach (Collider2D collider in attackList)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();
                if (damagable != null && collider.gameObject.layer == 8)
                {
                    damagable.TakeDamage(damage);
                }

                else
                {
                    break;
                }
            }
        }

        private IEnumerator DoDamage()
        {
            int i = 15;
            do
            {
                var hitList = OverlapRadius();
                yield return new WaitForSeconds(1f);
                Attack(hitList, Damage);
                var hitCircle = Instantiate(hitEffect, transform.position, Quaternion.identity);
                hitCircle.transform.localScale = new Vector2(attackradius, attackradius) * 2;
                yield return new WaitForSeconds(1f);
                Destroy(hitCircle);

            }
            while (i > 0);
        }

    }
}