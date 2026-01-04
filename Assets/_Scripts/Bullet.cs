using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactVFXPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(1);
        }

        if (enemy != null && !enemy.isShooted)
            enemy.Shooted();

        GameObject impactVFX = ObjectPool.instance.GetObject(impactVFXPrefab);
        impactVFX.transform.position = collision.contacts[0].point;

        ObjectPool.instance.DelayReturnToPool(gameObject);
        ObjectPool.instance.DelayReturnToPool(impactVFX, .1f);
    }
}
