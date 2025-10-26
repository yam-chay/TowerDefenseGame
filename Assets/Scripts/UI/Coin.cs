using UnityEngine;

namespace TDLogic
{
    public class Coin : MonoBehaviour
    {
        private Rigidbody2D rb;
        private bool inBag;
        private Animator animator;

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bag") && !inBag)
            {
                transform.localScale /= 1.5f;
                rb.linearVelocity = Vector2.down;
                inBag = true;
                animator.SetBool("inBag", true);
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                transform.position = new Vector2(collision.transform.position.x + 13, collision.transform.position.y + 10);
                rb.linearVelocity = Vector2.down;
            }


            if (collision.gameObject.CompareTag("Ground") && inBag)
            {
                transform.localScale *= 1.5f;
                animator.SetBool("inBag", false);
                inBag = false;
            }
        }
    }
}
