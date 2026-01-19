using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;

    bool isDead;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        Debug.Log("Health: " + health);

        if (IsDead())
            Die();
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public int GetHealth()
    {
        return health;
    }

    protected virtual void Die()
    {
    }

    private bool IsDead()
    {
        if (isDead) return false;

        if (health <= 0)
        {
            isDead = true;
            return true;
        }

        return false;
    }
}
