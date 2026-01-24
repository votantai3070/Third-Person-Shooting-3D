using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    private int currentHealth;

    [SerializeField] private int maxHealth = 100;

    public bool isDead { get; private set; }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (IsDead())
            Die();

        if (currentHealth < 0)
            currentHealth = 0;

        if (transform.GetComponent<Player>() != null)
            UI.instance.ingameUI.UpdateHealthUI(currentHealth, maxHealth);
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
