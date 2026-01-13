using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private LayerMask intersectionLayer;
    [SerializeField] private Collider[] intersectionCollider;
    [SerializeField] private Transform intersectionCheckParent;

    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();

        foreach (Collider collider in intersectionCollider)
        {
            Collider[] hits = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, intersectionLayer);

            foreach (Collider hit in hits)
            {
                Debug.Log("Hit detected: " + hit.name);

                IntersectionCheck intersectionCheck = hit.GetComponentInParent<IntersectionCheck>();

                if (intersectionCheck != null && intersectionCheckParent != intersectionCheck.transform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint enterPoint = GetEnterSnapPoint();

        AlignTo(enterPoint, targetSnapPoint); // Căn chỉnh rotation TRƯỚC
        SnapTo(enterPoint, targetSnapPoint);  // Ghép nối position SAU
    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Bước 1: Tính độ lệch rotation giữa snap point và object
        var rotationOffset =
            ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

        // Bước 2: Set rotation bằng với target
        transform.rotation = targetSnapPoint.transform.rotation;

        // Bước 3: Xoay 180° để đối diện nhau
        transform.Rotate(0, 180, 0);

        // Bước 4: Bù lại độ lệch ban đầu
        transform.Rotate(0, -rotationOffset, 0);
    }

    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Tính offset giữa tâm object và snap point riêng
        var offset = transform.position - ownSnapPoint.transform.position;

        // Vị trí mới = vị trí target + offset
        var newPosition = targetSnapPoint.transform.position + offset;

        // Di chuyển object đến vị trí mới
        transform.position = newPosition;
    }

    public SnapPoint GetEnterSnapPoint()
    {
        return GetSnapPointOfType(SnapPointType.Enter);
    }

    public SnapPoint GetExitSnapPoint()
    {
        return GetSnapPointOfType(SnapPointType.Exit);
    }

    private SnapPoint GetSnapPointOfType(SnapPointType type)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        //Collect snap points of the specified type
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.snapPointType == type)
            {
                filteredSnapPoints.Add(snapPoint);
            }
        }

        //Return a random snap point from the filtered list, or null if none found
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }

        return null;
    }
}
