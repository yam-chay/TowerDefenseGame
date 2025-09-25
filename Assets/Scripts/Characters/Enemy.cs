using System.Collections;
using UnityEngine;

namespace TDLogic
{
    /// <summary>
    /// Enemy character logic:
    /// - Waits in idle until player enters trigger.
    /// - Plays an alert animation before chasing the player.
    /// - Moves using Rigidbody2D and flips sprite based on direction.
    /// - Attacks targets in range periodically with a visual effect.
    /// </summary>
    public class Enemy : Character, IMovable
    {
        [Header("Enemy Settings")]
        [Tooltip("Horizontal move speed while chasing the player.")]
        public float moveSpeed = 2f;

        [Tooltip("Minimum distance to keep from the player before stopping.")]
        public float breakDistance = 1.5f;

        [Tooltip("Attack radius around the enemy.")]
        public float attackRadius = 1f;

        [Tooltip("Prefab for a hit effect spawned on each attack.")]
        public GameObject hitEffect;

        [Tooltip("Character data defining stats such as HP and damage.")]
        public CharacterData characterData;

        private SpriteRenderer sr;
        private Transform playerTransform;
        private Animator animator;
        private Rigidbody2D rb;

        // True when the enemy is in alert animation state
        private bool isAlert = false;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            // Initialize stats from data object
            if (characterData != null)
            {
                Init(characterData);
                HelloWorld(characterData.characterName);
            }

            // Start damage routine (looped attacks)
            StartCoroutine(DoDamage());
        }

        private void FixedUpdate()
        {
            if (isAlert)
            {
                Stop(); // Freeze during alert
            }
            else
            {
                MoveTo(); // Normal movement
                if (rb.linearVelocityX < 0)
                {
                    sr.flipX = true;
                }
                else if (rb.linearVelocityX > 0)
                {
                    sr.flipX = false;
                }
            }
        }

        /// <summary>
        /// Performs an attack on all valid targets in range.
        /// Spawns a visual effect at the enemy position.
        /// </summary>
        private void Attack()
        {
            Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, attackRadius);
            UtilsClass.Attack(hitList, Damage, gameObject);
        }

        /// <summary>
        /// Periodic attack coroutine (loops while enemy is alive).
        /// </summary>
        private IEnumerator DoDamage()
        {
            while (true)
            {
                Attack();

                // Visualize attack radius
               /* var hitCircle = Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
                hitCircle.transform.localScale = Vector2.one * attackRadius * 2f;
                Destroy(hitCircle, 0.1f);*/

                yield return new WaitForSeconds(0.5f); // Cooldown between attacks
            }
        }

        /// <summary>
        /// Stops enemy movement and updates animator speed.
        /// </summary>
        public void Stop()
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        }

        /// <summary>
        /// Moves the enemy towards the player if detected.
        /// Patrols in a straight line if no player assigned.
        /// </summary>
        public void MoveTo()
        {
            if (playerTransform != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                float distance = Vector2.Distance(playerTransform.position, transform.position);

                if (distance > breakDistance)
                {
                    // Chase the player
                    rb.linearVelocity = new Vector2(direction.x * moveSpeed * 2, rb.linearVelocity.y);
                    animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));

                    // Flip sprite to face target
                }
                else
                {
                    animator.SetTrigger("isAttacking");
                    Stop();
                }
            }
            else
            {
                // Simple patrol fallback (walk left)
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
            }
        }

        /// <summary>
        /// Called when player enters detection trigger.
        /// Plays alert animation and delays chase.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && Player.Instance != null)
            {
                animator.SetTrigger("alert");
                StartCoroutine(AlertRoutine());
                isAlert = true;
            }
        }

        /// <summary>
        /// Called when player leaves detection trigger.
        /// Clears player target reference.
        /// </summary>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && Player.Instance != null)
            {
                playerTransform = null;
            }
        }

        /// <summary>
        /// Alert state routine:
        /// Enemy freezes and plays alert animation before resuming chase.
        /// </summary>
        private IEnumerator AlertRoutine()
        {
            Stop();
            yield return new WaitForSeconds(1f); // Match alert animation length
            playerTransform = Player.Instance.transform;
            isAlert = false;
        }
    }
}
