using UnityEngine;

public class Footsteps_Visuals : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private TrailRenderer leftFootstepMarks;
    [SerializeField] private TrailRenderer rightFootstepMarks;

    [SerializeField] private LayerMask whatToLayerMask;
    [SerializeField] private float checkFootstepMarksRadius = .05f;

    private void Update()
    {
        CheckFootsteps(leftFootstepMarks);
        CheckFootsteps(rightFootstepMarks);
    }

    private void CheckFootsteps(TrailRenderer renderer)
    {
        Vector3 pos = renderer.transform.position;

        bool touchingGround = Physics.CheckSphere(pos, checkFootstepMarksRadius, whatToLayerMask);

        renderer.emitting = touchingGround;
    }

    private void OnDrawGizmos()
    {
        DrawFootGizmos(leftFootstepMarks.transform);
        DrawFootGizmos(rightFootstepMarks.transform);
    }

    private void DrawFootGizmos(Transform foot)
    {
        Gizmos.color = Color.yellow;
        Vector3 checkPos = foot.position;

        Gizmos.DrawWireSphere(checkPos, checkFootstepMarksRadius);
    }
}
