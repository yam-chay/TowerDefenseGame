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
        public CharacterData characterData;
        private SpriteRenderer sr;
        private Transform playerTransform;
        private Animator animator;
        private Rigidbody2D rb;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            if (characterData != null)
            {
                Init(characterData);
                HelloWorld(characterData.characterName);

            }
            Coroutine damageRoutine = StartCoroutine(DoDamage());
        }

        private void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, attackRadius);
            UtilsClass.Attack(hitList, Damage, gameObject);
        }

        private void FixedUpdate()
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
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetFloat("speed", rb.linearVelocityX);
        }

        //move to target
        public void MoveTo()
        {
            if (playerTransform != null)
            {
                // Calculate vector from enemy to player
                Vector2 direction = (playerTransform.position - transform.position).normalized;

                // Distance check
                float distance = Vector2.Distance(playerTransform.position, transform.position);

                if (distance > breakDistance)
                {
                    // Chase player
                    rb.linearVelocity = new Vector2(direction.x * moveSpeed * 2, rb.linearVelocity.y);
                    animator.SetFloat("speed", rb.linearVelocityX);

                    // Flip sprite
                    if (direction.x < 0)
                        sr.flipX = true;
                    else if (direction.x > 0)
                        sr.flipX = false;
                }
                else
                {
                    Stop();
                }
            }

            else
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                animator.SetFloat("speed", rb.linearVelocityX);
            }
        }
        //alert methode

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (Player.Instance != null)
                {
                    playerTransform = Player.Instance.transform;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (Player.Instance != null)
                {
                    playerTransform = null;
                }
            }
        }
    }
}

