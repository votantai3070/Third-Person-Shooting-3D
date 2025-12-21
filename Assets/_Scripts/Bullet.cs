using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactVFXPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject impactVFX = ObjectPool.instance.GetObject(impactVFXPrefab);
        impactVFX.transform.position = collision.contacts[0].point;

        ObjectPool.instance.DelayReturnToPool(gameObject);
        ObjectPool.instance.DelayReturnToPool(impactVFX, .1f);
    }
}
