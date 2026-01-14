using UnityEngine;

namespace KingdomScratch
{
    public class Coin : MonoBehaviour
    {
        [Header("Bag placement")]
        //Drop coins from height
        [SerializeField] private Vector3 dropPositionOffset = new Vector3(0, 3.5f, 0);
        //scale down size of coin sentering the bag
        [SerializeField] private float coinScaleDivisor = 1.9f;

        private CoinBag coinBag;
        private Rigidbody2D rb;
        private Animator animator;
        private bool inBag;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (CoinBag.Instance != null)
            {
                coinBag = CoinBag.Instance;
            }

            if (coinBag != null)
            {
                // Observe bag changes
                coinBag.OnCoinAdded += HandleCoinAdded;
                coinBag.OnCoinRemoved += HandleCoinRemoved;
            }

            inBag = false;
        }

        private void OnDestroy()
        {
            if (coinBag != null)
            {
                coinBag.OnCoinAdded -= HandleCoinAdded;
                coinBag.OnCoinRemoved -= HandleCoinRemoved;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !inBag)
            {
                coinBag.AddCoin(this);
            }

            if (collision.gameObject.CompareTag("Ground") && inBag)
            {
                coinBag.RemoveLastCoin();
            }
        }

        // Called when ANY coin is added to the bag
        private void HandleCoinAdded(Coin coin)
        {
            if (coin != this) return; // react only to self

            //drop the coin in the bag
            transform.position = coinBag.transform.position + dropPositionOffset;
            transform.localScale /= coinScaleDivisor;
            transform.parent = coinBag.transform;
            rb.linearVelocity = Vector2.down;
            animator.SetBool("inBag", true);  
            inBag = true;
        }

        // Called when ANY coin is removed from the bag
        private void HandleCoinRemoved(Coin coin)
        {
            if (coin != this) return; // react only to self

            transform.localScale *= coinScaleDivisor;
            transform.parent = null;
            animator.SetBool("inBag", false);
            inBag = false;
        }
    }
}
