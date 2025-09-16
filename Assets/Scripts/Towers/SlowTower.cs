using UnityEngine;
using System.Collections;

namespace TDLogic
{
    public class SlowTower : Tower
    {
        private string name;
        private int damage;
        private int health;
        private float radius;
        [SerializeField] private GameObject hitEffect;

        void Start()
        {
            this.health = 50;
            this.damage = 5;
            this.name = "slow tower";
            this.radius = 3f;
            HelloWorld(name);
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        public void Attack()
        {
            Collider2D[] hitList = CombatUtils.GetTargetsInRadius(transform.position, radius);
            CombatUtils.Attack(hitList, damage, gameObject);
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