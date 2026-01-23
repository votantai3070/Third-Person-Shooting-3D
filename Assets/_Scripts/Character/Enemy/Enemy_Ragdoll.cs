using UnityEngine;

public class Enemy_Ragdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollParent;

    Collider[] ragdollAllColliders;
    Rigidbody[] ragdollAllRigidbodies;

    private Rigidbody[] armRigidbodies;

    private void Start()
    {
        Transform armRoot = transform.Find("pelvis/spine_01/spine_02/spine_03/clavicle_r");
        if (armRoot != null)
        {
            armRigidbodies = armRoot.GetComponentsInChildren<Rigidbody>();
            DisableArmPhysics(); // Tắt physics của tay khi khởi động
        }
    }

    private void DisableArmPhysics()
    {
        foreach (Rigidbody rb in armRigidbodies)
        {
            rb.isKinematic = true; // Tắt physics, theo animation
        }
    }


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
