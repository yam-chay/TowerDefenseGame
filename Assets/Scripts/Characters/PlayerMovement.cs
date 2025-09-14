using UnityEngine;

namespace TDLogic
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private int health;
        private Rigidbody2D rb;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
           rb.linearVelocityX = Input.GetAxis("Horizontal") * speed;
        }
    }
}
