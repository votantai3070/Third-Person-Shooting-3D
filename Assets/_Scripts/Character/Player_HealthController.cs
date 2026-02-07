using UnityEngine;

public class Player_HealthController : MonoBehaviour, IDamageable
{
    public Player player;

    private int currentHealth;

    [SerializeField] private int maxHealth = 100;

    public bool isDead { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (IsDead())
            player.Die();

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
