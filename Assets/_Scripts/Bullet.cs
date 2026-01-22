using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactVFXPrefab;
    private float flyDistance = 50f;
    private Vector3 startPos;
    private Weapon_SO weaponData;

    private void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > flyDistance)
        {
            ObjectPool.instance.ReturnToPool(gameObject);
        }
    }

    public void SetupBullet(float flyDistance, Weapon_SO weaponData)
    {
        startPos = transform.position;
        this.flyDistance = flyDistance;
        this.weaponData = weaponData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null && weaponData != null)
            damageable.TakeDamage((int)weaponData.damage);

        if (enemy != null && !enemy.isShooted)
            enemy.Shooted();

        GameObject impactVFX = ObjectPool.instance.GetObject(impactVFXPrefab, null);
        impactVFX.transform.position = collision.contacts[0].point;

        ObjectPool.instance.DelayReturnToPool(gameObject);
        ObjectPool.instance.DelayReturnToPool(impactVFX, .1f);
    }
}
