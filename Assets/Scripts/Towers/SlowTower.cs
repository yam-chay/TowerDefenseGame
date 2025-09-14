using UnityEngine;
using System.Collections;

public class SlowTower : Tower
{
    private string name;
    private int damage;
    private int health;
    private float radius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.health = 50;
        this.damage = 5;
        this.name = "slow tower";
        this.radius = 3f;
        HelloWorld(name);
        Coroutine damageRoutine = StartCoroutine(DoDamage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoDamage()
    {
        int i = 15;
        do
        {
            var hitList = OverlapRadius(radius);
            yield return new WaitForSeconds(1f);
            Attack(hitList, damage);
        }
        while (i > 0);
    }
}
