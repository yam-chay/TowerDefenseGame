using UnityEngine;
using UnityEngine.UIElements;

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
                transform.localScale /= 2.5f;
                rb.linearVelocity = Vector2.zero;
                inBag = true;
                animator.SetBool("inBag", true);           
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !inBag)
            {
                transform.position = new Vector2(10.5f, 8);
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
