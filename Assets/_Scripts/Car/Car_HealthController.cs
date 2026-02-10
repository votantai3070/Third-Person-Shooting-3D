using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_HealthController : MonoBehaviour, IDamageable
{
    private Car_Controller car;

    public float currentHealth;
    public float maxHealth;

    public bool carBroken;

    [Header("Explosion info")]
    [SerializeField] private ParticleSystem explosionFx;
    [SerializeField] private ParticleSystem fireFx;
    [Space]
    [SerializeField] private GameObject explosionPoint;
    [SerializeField] private int explosionDamaged = 200;
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private float explosionForce = 7;
    [SerializeField] private float explosionUpwardsModifier = 2;
    [SerializeField] private float explosionRadius = 3;

    private void Start()
    {
        car = GetComponent<Car_Controller>();

        currentHealth = maxHealth;

    }

    private void Update()
    {
        if (fireFx.gameObject.activeSelf)
            fireFx.transform.rotation = Quaternion.identity;

        if (carBroken)
            car.rb.constraints = RigidbodyConstraints.None;
    }

    public void UpdateCarHealthUI()
    {
        UI.instance.ingameUI.UpdateCarHealthUI(currentHealth, maxHealth);
    }

    public void ReduceHealthCar(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0 && !carBroken)
            BrokenTheCar();

    }

    private void BrokenTheCar()
    {
        carBroken = true;
        car.GetBrokenTheCar();

        StartCoroutine(CarExplosionCo(explosionDelay));
    }

    public void TakeDamage(int damage)
    {
        ReduceHealthCar(damage);
        UpdateCarHealthUI();
    }

    private IEnumerator CarExplosionCo(float delay)
    {
        fireFx.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        car.rb.
            AddExplosionForce(explosionForce, explosionPoint.transform.position,
            explosionRadius, explosionUpwardsModifier, ForceMode.Impulse);

        explosionFx.gameObject.SetActive(true);

        Explosion();
    }

    private void Explosion()
    {
        HashSet<GameObject> uniqueEntity = new HashSet<GameObject>();

        Collider[] co = new Collider[32];
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, co);

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = co[i];
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                GameObject rootEntity = hit.transform.root.gameObject;

                if (uniqueEntity.Add(rootEntity) == false)
                    continue;

                damageable.TakeDamage(explosionDamaged);

                Rigidbody hitRb = hit.GetComponentInParent<Rigidbody>();

                Vector3 explosionPoint = transform.position + (Vector3.forward * 1.5f);
                hitRb.AddExplosionForce
                    (explosionForce, explosionPoint, 5, explosionUpwardsModifier, ForceMode.VelocityChange);
            }
        }
    }
}
