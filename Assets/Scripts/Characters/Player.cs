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

        [Header("Movement")]
        public float speed = 8;
        public float acceleration = 40f;    // smooth horizontal movement
        public float deceleration = 15f;    // smooth stopping
        [SerializeField] private float runModifier = 1.8f;
        private bool isRunning = false;


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
            float targetSpeed = isRunning ? speed * runModifier : speed ; 
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
            if (Input.GetKeyDown(KeyCode.H) && Health < characterData.health)
            {
                Heal(10);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Collider2D[] hitList = UtilsClass.GetTargetsInRadius(transform.position, characterData.range);
                UtilsClass.Attack(hitList, characterData.damage, gameObject);

            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawner.Spawn();
            }

            animator.SetFloat("velocity", Mathf.Abs(rb.linearVelocityX));

        }

        private void Interact()
        {
            Collider2D[] interactList = UtilsClass.GetTargetsInRadius(transform.position, characterData.range);
            UtilsClass.Interact(interactList, transform);
        }
    }
}
