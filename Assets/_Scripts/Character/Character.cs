using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;


    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        Debug.Log("Health: " + health);

        if (health <= 0)
            Die();
    }

    protected virtual void Die()
    {
    }
}
