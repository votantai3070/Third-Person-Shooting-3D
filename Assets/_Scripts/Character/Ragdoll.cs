using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollParent;

    Collider[] ragdollAllColliders;
    Rigidbody[] ragdollAllRigidbodies;

    private void Awake()
    {
        ragdollAllRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollAllColliders = GetComponentsInChildren<Collider>();

        RagdollActive(false);

        foreach (Rigidbody rb in ragdollAllRigidbodies)
            rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void RagdollActive(bool active)
    {
        foreach (var rigid in ragdollAllRigidbodies)
        {
            rigid.isKinematic = !active;
        }
    }

    public void CollidersActive(bool active)
    {
        foreach (var col in ragdollAllColliders)
        {
            col.enabled = active;
        }
    }
}
