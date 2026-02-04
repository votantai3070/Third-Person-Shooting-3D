using UnityEngine;

public class Car_DamageZone : MonoBehaviour
{
    private Car_Controller car;

    [SerializeField] float minSpeedToDamage = 1.5f;

    [SerializeField] int carDamage;
    [SerializeField] float impactForce = 150;
    [SerializeField] float upwardsMultiplier = 3;

    private void Awake()
    {
        car = GetComponentInParent<Car_Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (car.rb.linearVelocity.magnitude < minSpeedToDamage)
            return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable == null)
            return;

        damageable.TakeDamage(carDamage);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
            ApplyForce(rb);
    }

    private void ApplyForce(Rigidbody rigidbody)
    {
        rigidbody.isKinematic = false;
        rigidbody.AddExplosionForce(impactForce, transform.position, 3, upwardsMultiplier, ForceMode.Impulse);
    }
}
