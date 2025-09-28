using System.Collections;
using UnityEngine;

namespace TDLogic
{
    public enum EnemyState
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        Knockback
    }

    public class Enemy : Character
    {
        [Header("Enemy Settings")]
        public float moveSpeed = 2f;
        public float breakDistance = 1.5f;
        public GameObject hitEffect;
        public CharacterData characterData;

        [Header("Combat")]
        public float knockbackForce = 5f;
        public float knockbackDuration = 0.2f;
        public bool isAttack = false;

        [Header("Detection")]
        public float detectionRange = 5f;

        private SpriteRenderer sr;
        private Transform targetTransform;
        private Animator animator;
        private Rigidbody2D rb;
        private Coroutine patrolRoutine;
        private Coroutine attackRoutine;

        private EnemyState currentState;

        private void Start()
        {
            currentState = EnemyState.Patrol;
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            if (characterData != null)
            {
                Init(characterData);
                HelloWorld(characterData.characterName);
            }

            StartPatrol();
        }

        private void FixedUpdate()
        {
            switch (currentState)
            {
                case EnemyState.Patrol:
                    CheckForPlayer();
                    break;

                case EnemyState.Chase:
                    Chase();
                    CheckAttackRange();
                    break;

                case EnemyState.Attack:
                    if (!isAttack)
                    {
                        isAttack = true;
                        attackRoutine = StartCoroutine(DoDamage());
                        animator.SetTrigger("attack");
                    }
                    break;

                case EnemyState.Alert:
                    AlertRoutine();
                    break;

                case EnemyState.Knockback:
                    // During knockback, do nothing
                    break;
            }

            // Flip sprite based on velocity
            if (rb.linearVelocityX != 0)
            {
                sr.flipX = rb.linearVelocityX < 0;
            }

            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocityX));
        }

        #region Patrol / Chase / Attack
        private void CheckForPlayer()
        {
            if (targetTransform == null)
            {
                return;
            }

            float distance = Vector2.Distance(transform.position, targetTransform.position);
            if (distance <= detectionRange)
            {
                StopPatrol();
                currentState = EnemyState.Chase;
            }
        }

        private void Chase()
        {
            if (targetTransform == null)
            {
                currentState = EnemyState.Patrol;
                StartPatrol();
                return;
            }

            Vector2 dir = (targetTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(dir.x * moveSpeed * 2, rb.linearVelocity.y);
        }

        private void CheckAttackRange()
        {
            float distance = Vector2.Distance(transform.position, targetTransform.position);
            if (distance <= breakDistance)
            {
                currentState = EnemyState.Attack;
                if (attackRoutine == null)
                {
                    attackRoutine = StartCoroutine(DoDamage());
                }
            }
        }

        private IEnumerator DoDamage()
        {
            while (isAttack)
            {
                Attack();
                yield return new WaitForSeconds(1.5f);
                isAttack = false;
                StopCoroutine(attackRoutine);
            }
            attackRoutine = null;
        }

        public void StartPatrol()
        {
            currentState = EnemyState.Patrol;
            if (patrolRoutine == null)
                patrolRoutine = StartCoroutine(PatrolRoutine());
        }

        public void StopPatrol()
        {
            if (patrolRoutine != null)
            {
                StopCoroutine(patrolRoutine);
                patrolRoutine = null;
            }
            rb.linearVelocity = Vector2.zero;
        }

        private IEnumerator PatrolRoutine()
        {
            while (currentState == EnemyState.Patrol)
            {
                rb.linearVelocity = new Vector2(moveSpeed * Random.Range(-2f, 2f), rb.linearVelocity.y);
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            }
            patrolRoutine = null;
        }
        #endregion

        #region Knockback
        public void ApplyKnockback(Vector2 dir)
        {
            StopAllCoroutines();
            StartCoroutine(KnockbackRoutine(dir));
        }

        private IEnumerator KnockbackRoutine(Vector2 dir)
        {
            currentState = EnemyState.Knockback;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            animator.SetTrigger("isHurt");

            yield return new WaitForSeconds(knockbackDuration);

            rb.linearVelocity = Vector2.zero;
            currentState = EnemyState.Patrol;
            StartPatrol();
        }
        #endregion

        #region TakeDamage
        public override void TakeDamage(int damage, Transform attacker)
        {
            base.TakeDamage(damage, attacker);

            Vector2 dir = (transform.position - attacker.position).normalized;
            ApplyKnockback(dir);
        }
        #endregion

        #region TriggerDetection
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                targetTransform = collision.transform;
                currentState = EnemyState.Alert;
                StartCoroutine(AlertRoutine());
                StopPatrol();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                targetTransform = null;
                currentState = EnemyState.Patrol;
                StartPatrol();
            }
        }

        private IEnumerator AlertRoutine()
        {
            animator.SetTrigger("alert");
            yield return new WaitForSeconds(1f);
            if (targetTransform != null)
            {
                currentState = EnemyState.Chase;
            }
            else
            {
                currentState = EnemyState.Patrol;
            }
        }
        #endregion
    }
}
