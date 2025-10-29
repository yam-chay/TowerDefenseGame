using System.Collections;
using UnityEngine;

namespace KingdomScratch
{
    /// <summary>
    /// Controls enemy behavior including patrol, chase, attack, and death.
    /// Inherits logic from Character.
    /// </summary>
    public class Enemy : Character
    {
        [Header("Enemy Settings")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float breakDistance = 1.5f;
        [SerializeField] private CharacterData characterData;
        [SerializeField] private int lootDropCount = 2;

        [Header("References")]
        [SerializeField] private GameObject hitVFX;
        [SerializeField] private GameObject damageText;
        [SerializeField] private GameObject coins;

        [Header("Combat")]
        public bool isDead;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float knockUpForce = 0.2f;
        [SerializeField] private float knockbackDuration = 0.2f;

        [Header("Detection")]
        [SerializeField] private float detectionRange = 5f;
        private Transform playerTransform;

        //Components
        private SpriteRenderer sr;
        private Transform targetTransform;
        private Animator animator;
        private Rigidbody2D rb;

        //Coroutine
        private Coroutine patrolRoutine;
        private Coroutine attackRoutine;
        private Coroutine alertRoutine;
        private Coroutine knockbackRoutine;
        private Coroutine deathRoutine;

        private EnemyState currentState;

        private void Start()
        {
            playerTransform = Player.Instance.transform;

            //component initialization 
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            //intial enemy behaviour
            currentState = EnemyState.Patrol;
            Debug.Log(this);
            isDead = false;
            Init(characterData);
            

        }

        private void FixedUpdate()
        {
            switch (currentState)
            {
                case EnemyState.Patrol:
                    if (patrolRoutine == null)
                    {
                        patrolRoutine = StartCoroutine(PatrolRoutine());
                    }                    
                    CheckForPlayer();
                    break;

                case EnemyState.Chase:
                    if (patrolRoutine != null)
                    {
                        StopCoroutine(patrolRoutine);
                    }
                    Chase();
                    CheckAttackRange();
                    break;

                case EnemyState.Attack:
                    if (attackRoutine == null)
                    {
                        animator.SetTrigger(EnemyAnimatorParams.Attack);
                        attackRoutine = StartCoroutine(AttackRoutine());
                    }
                    break;

                case EnemyState.Alert:
                    if (alertRoutine == null)
                    {
                        animator.SetTrigger(EnemyAnimatorParams.Alert);
                        alertRoutine = StartCoroutine(AlertRoutine());
                    }
                    break;

                case EnemyState.Knockback:
                    if (knockbackRoutine == null && targetTransform)
                    {
                        Vector2 dir = new(targetTransform.GetComponent<IDamagable>().Damage / 10 * knockbackForce, knockUpForce);
                        knockbackRoutine = StartCoroutine(KnockbackRoutine(dir));
                    }
                    animator.SetTrigger(EnemyAnimatorParams.Hurt);
                    break;

                case EnemyState.Death:
                    StopAllCoroutines();
                    if (deathRoutine == null)
                    {
                        deathRoutine = StartCoroutine(DeathRoutine());
                    }
                    break;
            }

            // Flip sprite based on velocity
            if (rb.linearVelocityX != 0 && knockbackRoutine == null)
            {
                sr.flipX = rb.linearVelocityX < 0;
            }

            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocityX));
        }

        /// <summary>
        /// instantiate the visuals of the enemy getting hit
        /// </summary>
        private void HitVFX()
        {
            var offset = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(damageText, offset, Quaternion.identity, transform.parent);
        }

        //overrided to add state handling while taking damage.
        public override void TakeDamage(int damage, Transform attacker)
        {
            base.TakeDamage(damage, attacker);
            currentState = EnemyState.Knockback;
            HitVFX();

            if (Health <= 0 && !isDead)
            {
                currentState = EnemyState.Death;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        #region state machine methods
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
            else
            {
                Debug.LogWarning("player might be dead bro");
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
        #endregion

        #region IEnumerators
        private IEnumerator PatrolRoutine()
        {
            rb.linearVelocity = new Vector2(moveSpeed * Random.Range(-1.5f, 1.5f), 0);
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            patrolRoutine = null;
        }
        private IEnumerator AlertRoutine()
        {
            yield return new WaitForSeconds(1f);
            currentState = EnemyState.Chase;
            StopCoroutine(alertRoutine);
            alertRoutine = null;
        }

        private IEnumerator AttackRoutine()
        {
            Attack();
            yield return new WaitForSeconds(attackCooldown);
            StopCoroutine(attackRoutine);
            attackRoutine = null;
            CheckAttackRange();
        }

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

        private IEnumerator DeathRoutine()
        {
            isDead = true;
            for (int i = 0; i < lootDropCount; i++)
            {
                var coin = Instantiate(coins, transform.position, Quaternion.identity);
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(6, 10)), ForceMode2D.Impulse);
            }
            rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger(EnemyAnimatorParams.Death);
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }
        #endregion
    }
}