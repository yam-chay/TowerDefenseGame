using System.Collections;
using UnityEngine;

namespace TDLogic
{
    public class Enemy : Character
    {

        private float attackRadius;
        [SerializeField] private GameObject hitEffect;


        private void Start()
        {
            HelloWorld(name);
            attackRadius = 1;
            SetStats(50, 5, "Enemy");
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        public void Attack()
        {
            Collider2D[] hitList = CombatUtils.GetTargetsInRadius(transform.position, attackRadius);
            CombatUtils.Attack(hitList, Damage, gameObject);
        }

        //testing damage routine
        private IEnumerator DoDamage()
        {
            int i = 15;
            do
            {
                //attack sequence
                Attack();

                //visuals sequence
                var hitCircle = Instantiate(hitEffect, transform.position, Quaternion.identity);
                hitCircle.transform.localScale = new Vector2(attackRadius, attackRadius) * 2;
                yield return new WaitForSeconds(1f);
                Destroy(hitCircle);
                yield return new WaitForSeconds(1f);

            }
            while (i > 0);
        }

    }
}