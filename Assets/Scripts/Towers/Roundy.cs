using UnityEngine;
using System.Collections;

namespace KingdomScratch
{
    public class Roundy : Tower
    {
        [Header("Tower")]
        public TowerData towerData;
        [SerializeField] private GameObject hitEffect;

        void Start()
        {
            StartCoroutine(DoDamage());
            HelloWorld(towerData.name);
            Init(towerData);
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
                var hitCircle = Instantiate(towerData.hitEffect, transform.position + new Vector3(0.5f, 0.5f), Quaternion.identity.normalized, this.transform);
                hitCircle.transform.localScale = new Vector2(towerData.radius, towerData.radius);
                yield return new WaitForSeconds(0.2f);
                Destroy(hitCircle);
                yield return new WaitForSeconds(2f);

            }
            while (i > 0);
        }
    }
}