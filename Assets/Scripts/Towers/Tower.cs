using UnityEngine;

public class Tower : MonoBehaviour
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public float Radius { get; private set; }


    private void Start()
    {

    }

    private protected void HelloWorld(string name)
    {
        Debug.Log($"this is {name} Tower");
    }


    public Collider2D[] OverlapRadius(float radius)
    {
       Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, radius);

       foreach (Collider2D hit in hitList)
       {
           Debug.Log($"{hit} is in range");
       }

        return hitList;
    }

    private protected void Attack(Collider2D[] attackList,int damage)
    {
        foreach (Collider2D enemy in attackList)
        {
            if (enemy.GetComponent<Enemy>() == true)
            {
                enemy.GetComponent<IDamagable>().TakeDamage(damage);
            }
        }
    }

}
