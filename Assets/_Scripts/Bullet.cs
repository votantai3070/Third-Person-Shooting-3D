using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactVFXPrefab;
    private float flyDistance = 50f;
    private Vector3 startPos;

    private void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > flyDistance)
        {
            ObjectPool.instance.ReturnToPool(gameObject);
        }
    }

    public void SetupBullet(float flyDistance)
    {
        startPos = transform.position;
        this.flyDistance = flyDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        //Player player = collision.gameObject.GetComponentInParent<Player>();

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
