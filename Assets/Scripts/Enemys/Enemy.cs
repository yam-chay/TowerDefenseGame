using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public string Name { get; private set; }

    private protected void HelloWorld(string name)
    {
        Debug.Log($"this is {name} Enemy");
    }
}
