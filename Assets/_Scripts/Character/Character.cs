using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentHealth = 100;

    [SerializeField] private int maxHealth = 100;

    bool isDead;

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log("Health: " + currentHealth);

        if (IsDead())
            Die();

        //UI.instance.ingameUI.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    protected virtual void Die()
    {
    }

    private bool IsDead()
    {
        if (isDead) return false;

        if (currentHealth <= 0)
        {
            isDead = true;
            return true;
        }

        return false;
    }
}
