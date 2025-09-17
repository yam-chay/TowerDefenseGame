using System.Collections;
using UnityEngine;

namespace TDLogic
{
    public class Enemy : Character, IMovable
    {
        [Header("Enemy Settings")]
        public float moveSpeed;
        public float breakDistance;
        public float attackRadius;
        public GameObject hitEffect;
        private Transform target;

        private void Awake()
        {
            if (Player.Instance != null)
            {
                target = Player.Instance.transform;
            }
        }

        private void Start()
        {
            HelloWorld(name);
            SetStats("Enemy", 50, 5);
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        private void over ()
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
                hitCircle.transform.parent = transform;
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
            if (Mathf.Abs(distanceX) < breakDistance)
            {
                return;
            }

            float direction = Mathf.Sign(distanceX);
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }


    }
}