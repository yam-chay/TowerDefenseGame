using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KingdomScratch
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        [Header("Data")]
        [Tooltip("Character configuration data including health, range, damage and name.")]
        public CharacterData characterData;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private CoinBag coinBag;
        private Spawner spawner;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Animator animator;
        private Coroutine damageEffect;

        [Header("Movement")]

        //Base walking speed of the player
        public float speed = 8f;

        //How quickly the player accelerates to max speed
        public float acceleration = 40f;

        //How quickly the player slows down when movement input stops
        public float deceleration = 15f;

        //Run speed multiplier when holding Shift
        [SerializeField] private float runModifier = 1.8f;

        [Header("Animations Queue")]
        private bool isRunning = false;
        private bool isDead = false;
        private bool isHurt = false;

        /// <summary>
        /// Global access to the player instance.
        /// </summary>
        public static Player Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            Init(characterData);
        }

        private void Start()
        {
            //testing tool
            if (Spawner.Instance != null)
            {
                spawner = Spawner.Instance;
            }

            //character say hello on console
            Debug.Log(this);

            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (!isHurt)
            {
                Move();
            }
        }

        void Update()
        {
            healthSlider.value = Health;

            //check run input
            isRunning = Input.GetKey(KeyCode.LeftShift);

            //input system (old)
            if (isDead)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.H) && Health < characterData.health)
            {
                Heal(10);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawner.Spawn();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("attack", true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("attack", false);
            }

            animator.SetFloat("velocity", Mathf.Abs(rb.linearVelocityX));
        }

        /// <summary>
        /// allows the player to move smoothly with some math
        /// </summary>
        private void Move()
        {
            // Calculate target velocity
            float targetSpeed = isRunning ? speed * runModifier : speed;
            float targetVelX = Input.GetAxisRaw("Horizontal") * targetSpeed;
            float smooth = Mathf.Abs(targetVelX) > 0.01f ? acceleration : deceleration;

            //create a smooth velocity transition taking into account accelerating/decelerating 
            float velX = Mathf.MoveTowards(rb.linearVelocityX, targetVelX, smooth * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(velX, rb.linearVelocityY);

            // Flip sprite based on movement direction
            if (rb.linearVelocityX < 0)
            {
                sr.flipX = true;
            }
            else if (rb.linearVelocityX > 0)
            {
                sr.flipX = false;
            }
        }

        /// <summary>
        /// Handles incoming damage and triggers death or hit reactions.
        /// </summary>
        public override void TakeDamage(int damage, Transform attacker)
        {
            base.TakeDamage(damage, attacker);

            if (Health <= 0)
            {
                Die();
            }

            else if (damageEffect == null)
            {
                isHurt = true;
                damageEffect = StartCoroutine(DamageEffect());
            }
        }

        /// <summary>
        /// death sequence
        /// </summary>
        private void Die()
        {
            isDead = true;
            speed = 0;
            animator.SetBool("isDead", true);
            gameOverUI.gameObject.SetActive(true);
        }

        /// <summary>
        /// interaction method
        /// </summary>
        private void Interact()
        {
            Collider2D[] interactList = UtilsClass.GetTargetsInRadius(transform.position, characterData.range);
            UtilsClass.Interact(interactList, transform);
        }


        /// <summary>
        /// the methode that is called whenever a certain frame ot the attack animation is playing
        /// </summary>
        public void ComboAttack()
        {
            Attack();
        }

        /// <summary>
        /// a sequence of the damage effect visuals display
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageEffect()
        {
            rb.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
            animator.SetBool("isHit", true);
            sr.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            sr.color = Color.white;
            coinBag.RemoveCoin();
            var coinRb = Instantiate(coinPrefab, transform.position + Vector3.up * 2, Quaternion.identity).GetComponent<Rigidbody2D>();
            coinRb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.35f);
            isHurt = false;
            animator.SetBool("isHit", false);
            StopCoroutine(damageEffect);
            damageEffect = null;
        }
    }
}
