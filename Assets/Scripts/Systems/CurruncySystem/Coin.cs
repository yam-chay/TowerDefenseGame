using UnityEngine;

namespace KingdomScratch
{
    public class Coin : MonoBehaviour
    {
        private CoinBag coinBag;
        private Vector3 dropPositionOffset;
        private float coinScaleOffset;

        private Rigidbody2D rb;
        private Animator animator;
        private bool inBag;

        private void Start()
        {
            dropPositionOffset = new Vector3(0, 3.5f, 0);
            coinScaleOffset = 1.9f;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            coinBag = FindObjectOfType<CoinBag>();
            inBag = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !inBag)
            {
                coinBag.AddCoin(this);                
                transform.position = coinBag.transform.position + dropPositionOffset;
                transform.localScale /= coinScaleOffset;
                transform.parent = coinBag.transform;
                rb.linearVelocity = Vector2.down;
                animator.SetBool("inBag", true);
                inBag = true;
            }

            if (collision.gameObject.CompareTag("Ground") && inBag)
            {
                coinBag.RemoveCoin();
                transform.localScale *= coinScaleOffset;
                transform.parent = null;
                animator.SetBool("inBag", false);
                inBag = false;
            }
        }
    }
}
