using UnityEngine;

public class Car_HealthController : MonoBehaviour, IDamageable
{
    private Car_Controller car;

    public float currentHealth;
    public float maxHealth;

    private bool carBroken;

    private void Start()
    {
        car = GetComponent<Car_Controller>();

        currentHealth = maxHealth;
    }

    public void UpdateCarHealthUI()
    {
        UI.instance.ingameUI.UpdateCarHealthUI(currentHealth, maxHealth);
    }

    public void ReduceHealthCar(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            BrokenTheCar();

    }

    private void BrokenTheCar()
    {
        carBroken = true;
        car.GetBrokenTheCar();
        // enable smoke
        // invoke explosion
    }

    public void TakeDamage(int damage)
    {
        ReduceHealthCar(damage);
        UpdateCarHealthUI();
    }
}
