using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TDLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        [Header("Data")]
        public CharacterData characterData;
        private Spawner spawner;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Animator animator;
        [SerializeField] private Slider slider;
        [SerializeField] private GameObject gameOver;

        [Header("Movement")]
        public float speed = 8;
        public float acceleration = 40f;    // smooth horizontal movement
        public float deceleration = 15f;    // smooth stopping
        [SerializeField] private float runModifier = 1.8f;
        private bool isRunning = false;

        private Coroutine damageEffect;
        private bool isDead = false;

        public static Player Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            if (Spawner.Instance != null)
            {
                spawner = Spawner.Instance;
            }

            Init(characterData);
            HelloWorld(characterData.characterName);
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            slider.value = Health;
            isRunning = Input.GetKey(KeyCode.LeftShift);

            //horizontal movement
            float targetSpeed = isRunning ? speed * runModifier : speed;
            float targetVelX = Input.GetAxisRaw("Horizontal") * targetSpeed;
            float smooth;

            //decide if player is accelerating or decelerating
            if (Mathf.Abs(targetVelX) > 0.01f)
            {
                smooth = acceleration;
            }
            else
            {
                smooth = deceleration;
            }

            //create a smooth float points to move through while accelerating/decelerating 
            float velX = Mathf.MoveTowards(rb.linearVelocityX, targetVelX, smooth * Time.fixedDeltaTime);

            //move the player to each point 
            rb.linearVelocity = new Vector2(velX, rb.linearVelocityY);
            if (rb.linearVelocityX < 0)
            {
                sr.flipX = true;
            }
            else if (rb.linearVelocityX > 0)
            {
                sr.flipX = false;
            }
        }

        void Update()
        {
            if (!isDead)
            {
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

                if (Input.GetKeyDown(KeyCode.Space) && !isDead)
                {
                    animator.SetBool("attack", true);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    animator.SetBool("attack", false);
                }

                animator.SetFloat("velocity", Mathf.Abs(rb.linearVelocityX));
            }
        }

        public override void TakeDamage(int damage, Transform attacker)
        {
            base.TakeDamage(damage, attacker);

            if (Health <= 0)
            {
                //Game Over Screen
                isDead = true;
                speed = 0;
                animator.SetBool("isDead", true);
                gameOver.gameObject.SetActive(true);
            }

            else if (damageEffect == null)
            {
                damageEffect = StartCoroutine(DamageEffect());
            }
        }

        private void Interact()
        {
            Collider2D[] interactList = UtilsClass.GetTargetsInRadius(transform.position, characterData.range);
            UtilsClass.Interact(interactList, transform);
        }

        //used by animator events
        public void ComboAttack()
        {
            Attack();
        }

        private IEnumerator DamageEffect()
        {
            animator.SetBool("isHit", true);
            sr.color = Color.red;
            rb.AddForce(new Vector2(-rb.linearVelocityX, 0f) ,ForceMode2D.Impulse); 
            yield return new WaitForSeconds(0.15f);
            sr.color = Color.white;
            animator.SetBool("isHit", false);
            StopCoroutine(damageEffect);
            damageEffect = null;
        }
    }
}
