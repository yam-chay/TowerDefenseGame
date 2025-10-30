using UnityEngine;

namespace KingdomScratch
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Transform coinBag;
        private Rigidbody2D rb;
        private Animator animator;
        private bool inBag;
        private bool spawned;

        private void Awake()
        {
            spawned = true;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {

            spawned = false;
            inBag = false;
            animator.SetBool("inBag", false);
        }

        private void OnEnable()
        {
            rb.AddForce(new Vector2(5, 6), ForceMode2D.Impulse);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bag") && !inBag)
            {
                transform.parent = coinBag;
                if (!inBag)
                {
                    transform.localScale /= 1.5f;
                }
                inBag = true;
                animator.SetBool("inBag", true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bag") && inBag)
            {
                transform.parent = null;
                if (inBag)
                {
                    transform.localScale *= 1.5f;
                }
                rb.linearVelocity = Vector2.down;
                inBag = false;
                animator.SetBool("inBag", false);
            }
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !spawned)
            {
                transform.position = new Vector2(collision.transform.position.x + 13, collision.transform.position.y + 11);
                rb.linearVelocity = Vector2.down;
            }
        }
    }
}
