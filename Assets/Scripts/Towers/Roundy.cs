using UnityEngine;
using System.Collections;

namespace TDLogic
{
    public class Roundy : Tower
    {
        [Header("Tower")]
        [SerializeField] private GameObject hitEffect;

        void Start()
        {
            SetStats("Roundy", 100, 10, 3);
            HelloWorld(Name);
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        public void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, Radius);
            UtilsClass.Attack(hitList, Damage, gameObject);
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
                hitCircle.transform.localScale = new Vector2(Radius, Radius) * 2;
                yield return new WaitForSeconds(1f);
                Destroy(hitCircle);
                yield return new WaitForSeconds(1f);

            }
            while (i > 0);
        }
    }
}