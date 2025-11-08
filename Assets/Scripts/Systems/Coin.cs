using UnityEngine;

namespace KingdomScratch
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] public Transform coinBag;
        [SerializeField] public Transform dropPoint;
        [SerializeField] public Transform coinPool;
        private Rigidbody2D rb;
        private Animator animator;
        private bool inBag;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
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
                if (!inBag)
                {
                    transform.localScale /= 1.7f;
                }
                inBag = true;
                animator.SetBool("inBag", true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bag") && inBag)
            {
                if (inBag)
                {
                    transform.localScale *= 1.7f;
                }
                transform.parent = coinPool;
                rb.AddForce(Vector2.down * 3, ForceMode2D.Impulse);
                inBag = false;
                animator.SetBool("inBag", false);
            }
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                transform.position = dropPoint.position;
                rb.linearVelocity = Vector2.down;
                transform.parent = coinBag;
            }
        }
    }
}
