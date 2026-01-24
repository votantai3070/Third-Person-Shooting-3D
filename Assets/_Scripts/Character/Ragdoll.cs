using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollParent;

    Collider[] ragdollAllColliders;
    Rigidbody[] ragdollAllRigidbodies;

    public void RagdollActive(bool active)
    {
        ragdollAllRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (var rigid in ragdollAllRigidbodies)
        {
            rigid.isKinematic = !active;
        }
    }

    public void CollidersActive(bool active)
    {
        ragdollAllColliders = GetComponentsInChildren<Collider>();

        foreach (var col in ragdollAllColliders)
        {
            col.enabled = active;
        }
    }
}
