using UnityEngine;
using System.Collections;

namespace TDLogic
{
    public class Roundy : Tower, IInteractable
    {
        [Header("Tower")]
        [SerializeField] private GameObject hitEffect;
        [SerializeField] GameObject upgradeMenu;
        [SerializeField] private string _name;
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] private float _range;

        void Start()
        {
            upgradeMenu.SetActive(false);
            SetStats(_name, _health, _damage, _range);
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
                var hitCircle = Instantiate(hitEffect, transform.position, Quaternion.identity.normalized, this.transform);
                hitCircle.transform.localScale = new Vector2(Radius, Radius) * 2;
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