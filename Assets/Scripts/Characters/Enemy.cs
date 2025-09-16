using System.Collections;
using UnityEngine;

namespace TDLogic
{
    public class Enemy : Character, IMovable
    {
        [Header("Enemy Settings")]
        public float moveSpeed;
        public float attackRadius;
        public GameObject hitEffect;
        private Transform target = Player.Instance.transform;



        private void Start()
        {
            target = Player.Instance.transform;
            HelloWorld(name);
            SetStats("Enemy", 50, 5);
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        public void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, attackRadius);
            UtilsClass.Attack(hitList, Damage, gameObject);
        }

        private void Update()
        {
            MoveTo();
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


        public void Stop()
        {
            //stop moving
        }

        //move to target
        public void MoveTo()
        {
            if (target == null)
            {
                Debug.LogWarning($"{name} has no target assigned!");
                return;
            }

            float distanceX = target.position.x - transform.position.x;
            if (Mathf.Abs(distanceX) < 5f)
            {
                return;
            }

            float direction = Mathf.Sign(distanceX);
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }


    }
}