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

        [Header("Detection")]
        public float detectionRange = 5f;
        private Transform playerTransform;

        private SpriteRenderer sr;
        private Transform targetTransform;
        private Animator animator;
        private Rigidbody2D rb;
        private Coroutine patrolRoutine;
        private Coroutine attackRoutine;
        private Coroutine alertRoutine;
        private Coroutine knockbackRoutine;

        private EnemyState currentState;

        private void Awake()
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
        }

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            currentState = EnemyState.Patrol;

            if (characterData != null)
            {
                Init(characterData);
                HelloWorld(characterData.characterName);
            }
        }

        private void FixedUpdate()
        {
            switch (currentState)
            {
                case EnemyState.Patrol:
                    StartPatrol();
                    CheckForPlayer();
                    break;

                case EnemyState.Chase:
                    StopPatrol();
                    Chase();
                    CheckForPlayer();
                    CheckAttackRange();
                    break;

                case EnemyState.Attack:
                    if (attackRoutine == null)
                    {
                        animator.SetTrigger("attack");
                        attackRoutine = StartCoroutine(DoDamage());
                    }
                    CheckAttackRange();
                    break;

                case EnemyState.Alert:
                    if (alertRoutine == null)
                    {
                        alertRoutine = StartCoroutine(AlertRoutine());
                    }
                    break;

                case EnemyState.Knockback:
                    if (knockbackRoutine == null)
                    {
                        Vector2 dir = new Vector2(targetTransform.GetComponent<IDamagable>().Damage, 0);
                        knockbackRoutine = StartCoroutine(KnockbackRoutine(dir));
                    }
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
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            if (distance <= detectionRange)
            {
                targetTransform = playerTransform;
                currentState = EnemyState.Alert;
            }
            else
            {
                currentState = EnemyState.Patrol;
            }
        }

        private void Chase()
        {
            Vector2 dir = (targetTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(dir.x * moveSpeed * 2, rb.linearVelocity.y);
        }

        private void CheckAttackRange()
        {
            float distance = Vector2.Distance(transform.position, targetTransform.position);
            if (distance <= breakDistance)
            {
                currentState = EnemyState.Attack;
            }
            else if (distance >= detectionRange)
            {
                currentState = EnemyState.Patrol;
            }
            else
            {
                currentState = EnemyState.Chase;
            }
        }

        private IEnumerator DoDamage()
        {
            Attack();
            yield return new WaitForSeconds(0.5f);
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        public void StartPatrol()
        {
            if (patrolRoutine == null)
            {
                patrolRoutine = StartCoroutine(PatrolRoutine());
            }
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
            rb.linearVelocity = new Vector2(moveSpeed * Random.Range(-1.5f, 1.5f), 0);
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            patrolRoutine = null;
        }
        #endregion

        #region Knockback

        private IEnumerator KnockbackRoutine(Vector2 dir)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(-dir * knockbackForce, ForceMode2D.Impulse);
            animator.SetTrigger("isHurt");

            yield return new WaitForSeconds(knockbackDuration);

            rb.linearVelocity = Vector2.zero;
            CheckAttackRange();
            StopCoroutine(knockbackRoutine);
            knockbackRoutine = null;
        }
        #endregion

        public override void TakeDamage(int damage, Transform attacker)
        {
            base.TakeDamage(damage, attacker);

            Vector2 dir = (transform.position - attacker.position).normalized;
            currentState = EnemyState.Knockback;
        }

        private IEnumerator AlertRoutine()
        {
            animator.SetTrigger("alert");
            currentState = EnemyState.Chase;
            yield return new WaitForSeconds(1f);
            StopCoroutine(alertRoutine);
            alertRoutine = null;
        }
    }
}
