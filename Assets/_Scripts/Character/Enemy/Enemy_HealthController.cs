using UnityEngine;

public class Enemy_HealthController : MonoBehaviour, IDamageable
{
    public Enemy enemy;
    public Enemy_Melee enemyMelee;
    public Enemy_Range enemyRange;

    private int currentHealth;

    [SerializeField] private int maxHealth = 100;

    public bool isDead { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyMelee = GetComponent<Enemy_Melee>();
        enemyRange = GetComponent<Enemy_Range>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (IsDead())
            if (enemyRange != null)
                enemyRange.Die();
            else if (enemyMelee != null)
                enemyMelee.Die();

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
