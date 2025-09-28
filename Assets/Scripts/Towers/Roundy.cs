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
                var hitCircle = Instantiate(towerData.hitEffect, transform.position + new Vector3(0.5f, 0.5f), Quaternion.identity.normalized, this.transform);
                hitCircle.transform.localScale = new Vector2(towerData.radius, towerData.radius);
                yield return new WaitForSeconds(0.2f);
                Destroy(hitCircle);
                yield return new WaitForSeconds(2f);

            }
            while (i > 0);
        }

        public void Interact(Transform interactor)
        {
            upgradeMenu.SetActive(!upgradeMenu.activeSelf);
            if (upgradeMenu.activeSelf)
            {
                Time.timeScale = 0.1f;
            }
            else
            {
                Time.timeScale = 1f;
            }    
        }
    }
}