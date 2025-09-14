using UnityEngine;

public class SlowEnemy : Enemy, IDamagable
{
    private string name;
    private int health;
    private int damage;

    private void Start()
    {
        this.name = "Slow Enemy";
        this.health = 50;
        this.damage = 5;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining: " + health);

        if (health <= 0)
        {
            Die();
        }

    }
    private void Die()
    {
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }
}
