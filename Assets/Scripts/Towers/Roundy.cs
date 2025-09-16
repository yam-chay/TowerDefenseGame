using UnityEngine;
using System.Collections;

namespace TDLogic
{
    public class Roundy : Tower
    {
        [Header("Tower")]
        [SerializeField] private string name;
        [SerializeField] private int damage;
        [SerializeField] private int health;
        [SerializeField] private float radius;
        [SerializeField] private GameObject hitEffect;

        void Start()
        {
            this.health = Health;
            this.damage = Damage;
            this.name = Name;
            this.radius = Radius;
            HelloWorld(name);
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        public void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, radius);
            UtilsClass.Attack(hitList, damage, gameObject);
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
                hitCircle.transform.localScale = new Vector2(radius, radius) * 2;
                yield return new WaitForSeconds(1f);
                Destroy(hitCircle);
                yield return new WaitForSeconds(1f);

            }
            while (i > 0);
        }
    }
}