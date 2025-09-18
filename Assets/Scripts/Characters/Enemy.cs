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
        [SerializeField] private Transform playerTransform;
        [SerializeField] private string _name;
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] SpriteRenderer sr;

        private void Start()
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }

            HelloWorld(name);
            SetStats(_name, _health, _damage);
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        private void over ()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, attackRadius);
            UtilsClass.Attack(hitList, Damage, gameObject);
        }

        private void Update()
        {
            if (playerTransform != null)
            {                
                MoveTo();
            }
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
                var hitCircle = Instantiate(hitEffect, transform.position, Quaternion.identity, this.transform);
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
            if (playerTransform == null)
            {
                Debug.LogWarning($"{name} has no target assigned!");
                return;
            }

            float distanceX = playerTransform.position.x - transform.position.x;
            if (Mathf.Abs(distanceX) < breakDistance)
            {
                return;
            }

            float direction = Mathf.Sign(distanceX);
            if (direction <= 0)
            {
                 sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }

            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }


    }
}