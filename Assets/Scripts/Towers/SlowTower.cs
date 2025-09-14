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

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator DoDamage()
        {
            int i = 15;
            do
            {
                var hitList = OverlapRadius(radius);
                yield return new WaitForSeconds(1f);
                Attack(hitList, damage);
                var hitCircle = Instantiate(hitEffect, transform.position, Quaternion.identity);
                hitCircle.transform.localScale = new Vector2(radius, radius)*2;
                yield return new WaitForSeconds(1f);
                Destroy(hitCircle);

            }
            while (i > 0);
        }
    }
}