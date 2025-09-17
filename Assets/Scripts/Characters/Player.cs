using UnityEngine;

namespace TDLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {

        private Rigidbody2D rb;

        [Header("Movement")]
        public float speed = 8;
        public float acceleration = 40f;    // smooth horizontal movement
        public float deceleration = 15f;    // smooth stopping

        [Header("Jump")]
        public float jumpForce = 8;
        public float jumpDuration = 0.3f;
        public float fallingMultiplier = 5f;   //smooth Falling
        private bool isGrounded;
        private bool isJumping;
        private float jumpTime;

        [Header("Interaction")]
        [SerializeField] private float interactRange;


        public static Player Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            SetStats("The Player", 100, 5);
            HelloWorld(name);
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            //horizontal movement
            float targetVelX = Input.GetAxisRaw("Horizontal") * speed;
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

            //gravity handler
            if (rb.linearVelocity.y < 0f)
            {
                rb.linearVelocity += Vector2.up * rb.gravityScale * -fallingMultiplier * Time.fixedDeltaTime;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && isGrounded)
            {
                Heal(10);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }

                //jump start
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isJumping = true;
                jumpTime = 0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            //while jumping
            if (Input.GetKey(KeyCode.Space) && isJumping)
            {
                if (jumpTime < jumpDuration)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
                    jumpTime += Time.deltaTime;
                }
            }

            //jump ends
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }

        private static void Attack()
        {
            ;
        }

        private void Interact()
        {
            Collider2D[] interactList = UtilsClass.GetTargetsInRadius(transform.position, interactRange);
            UtilsClass.Interact(interactList, transform);
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
                isJumping = false;
                jumpTime = 0f;
            }

            else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                isGrounded = false;
            }
        }
    }
}
