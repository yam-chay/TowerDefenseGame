using UnityEngine;

namespace TDLogic
{
    public class Player : Character
    {
        private Rigidbody2D rb;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            HelloWorld(name);
            SetStats(100, 5, "The Player", 9 , 7);
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            //horizontal movement
            rb.linearVelocityX = Input.GetAxis("Horizontal") * Speed;

            //junp mechanic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpHeight);
            }
        }
        private void Attack(GameObject target)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(Damage);
            }
        }




    }
}
