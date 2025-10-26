using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace TDLogic
{
    public enum EnemyState
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        Knockback,
        Death,
    }

    public class Enemy : Character
    {
        [Header("Enemy Settings")]
        public float moveSpeed = 2f;
        public float breakDistance = 1.5f;
        public GameObject hitEffect;
        public GameObject hitEffectDMG;
        public GameObject coins;
        public CharacterData characterData;
        [SerializeField] private int lootDrop = 2;

        [Header("Combat")]
        public float attackCooldown = 0.5f;
        public float knockbackForce = 5f;
        public float knockUpForce = 0.2f;
        public float knockbackDuration = 0.2f;
        public bool isDead;

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

        private void Start()
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
            isDead = false;

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
                    //CheckForPlayer();
                    CheckAttackRange();
                    break;

                case EnemyState.Attack:
                    if (attackRoutine == null)
                    {
                        animator.SetTrigger("attack");
                        attackRoutine = StartCoroutine(AttackRoutine());
                    }
                    break;

                case EnemyState.Alert:
                    if (alertRoutine == null)
                    {
                        animator.SetTrigger("alert");
                        alertRoutine = StartCoroutine(AlertRoutine());
                    }
                    break;

                case EnemyState.Knockback:
                    if (knockbackRoutine == null && targetTransform)
                    {
                        Vector2 dir = new(targetTransform.GetComponent<IDamagable>().Damage / 10 * knockbackForce, knockUpForce);
                        knockbackRoutine = StartCoroutine(KnockbackRoutine(dir));
                    }
                    animator.SetTrigger("isHurt");
                    break;

                case EnemyState.Death:
                    Destroy(gameObject.GetComponent<Collider2D>());
                    StopAllCoroutines();
                    animator.SetTrigger("death");
                    Destroy(gameObject, 1);
                    break;
            }

            // Flip sprite based on velocity
            if (rb.linearVelocityX != 0 && knockbackRoutine == null)
            {
                sr.flipX = rb.linearVelocityX < 0;
            }

            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocityX));
        }

        #region Patrol / Chase / Attack
        private void CheckForPlayer()
        {
            if (playerTransform)
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
        }

        private void Chase()
        {
            if (targetTransform)
            {
                Vector2 dir = (targetTransform.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(dir.x * moveSpeed * 2.5f, 0);
            }
        }

        private void CheckAttackRange()
        {
            if (targetTransform)
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
        }

        private IEnumerator AttackRoutine()
        {
            Attack();
            yield return new WaitForSeconds(attackCooldown);
            StopCoroutine(attackRoutine);
            attackRoutine = null;
            CheckAttackRange();
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

        private IEnumerator KnockbackRoutine(Vector2 dir)
        {
            rb.linearVelocity = Vector2.zero;
            rb.freezeRotation = false;
            rb.AddForce(-dir, ForceMode2D.Impulse);
            sr.color = Color.red;
            yield return new WaitForSeconds(knockbackDuration / 2);
            sr.color = Color.white;
            yield return new WaitForSeconds(knockbackDuration / 2);
            transform.rotation = Quaternion.identity;
            rb.freezeRotation = true;
            rb.linearVelocity = Vector2.zero;
            CheckAttackRange();
            StopCoroutine(knockbackRoutine);
            knockbackRoutine = null;
        }

        public override void TakeDamage(int damage, Transform attacker)
        {
            currentState = EnemyState.Knockback;
            HitEffectPopUp();

            Health -= damage;
            if (Health <= 0 && !isDead)
            {
                for (int i = 0; i < lootDrop; i++)
                {
                    isDead = true;
                    var coin = Instantiate(coins, transform.position, Quaternion.identity);
                    coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(6,10)), ForceMode2D.Impulse);
                }

                rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                currentState = EnemyState.Death;
            }
        }

        private void HitEffectPopUp()
        {
            var offset = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(hitEffectDMG, offset, Quaternion.identity, transform.parent);
        }

        private IEnumerator AlertRoutine()
        {
            yield return new WaitForSeconds(1f);
            currentState = EnemyState.Chase;
            StopCoroutine(alertRoutine);
            alertRoutine = null;
        }
    }
}
