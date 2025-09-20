using UnityEngine;
using System.Collections;

namespace TDLogic
{
    public class Roundy : Tower, IInteractable
    {
        [Header("Tower")]
        [SerializeField] private TowerData towerData;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] GameObject upgradeMenu;
        private Coroutine damageRoutine;

        void Start()
        {
            upgradeMenu.SetActive(false);
            damageRoutine = StartCoroutine(DoDamage());
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
                var hitCircle = Instantiate(towerData.hitEffect, transform.position, Quaternion.identity.normalized, this.transform);
                hitCircle.transform.localScale = new Vector2(towerData.radius, towerData.radius) * 2;
                yield return new WaitForSeconds(1f);
                Destroy(hitCircle);
                yield return new WaitForSeconds(1f);

            }
            while (i > 0);
        }

        public void Interact(Transform interactor)
        {
            upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        }
    }
}