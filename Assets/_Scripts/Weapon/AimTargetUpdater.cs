using UnityEngine;

public class AimTargetUpdater : MonoBehaviour
{
    public Player player { get; private set; }

    [Header("References")]
    [SerializeField] private Transform aimTarget; // AimTarget GameObject
    [SerializeField] private Camera playerCamera;

    [Header("Settings")]
    [SerializeField] private LayerMask aimLayerMask = ~0;
    [SerializeField] private float maxAimDistance = 1000f;
    [SerializeField] private float defaultAimDistance = 50f;

    private void Awake()
    {
        player = GetComponentInParent<Player>();

        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
    }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        // Setup initial position
        InitializeAimTarget();
    }

    private void Update()
    {
        UpdateAimTargetPosition();
    }

    void InitializeAimTarget()
    {
        if (player == null || player.visuals == null)
        {
            Debug.LogWarning("Player or visuals not found!");
            return;
        }

        Transform viewPoint = player.visuals.GetPlayerViewPointTransform();

        if (viewPoint == null)
        {
            Debug.LogWarning("ViewPoint transform is null!");
            return;
        }

        // Set AimTarget initial position phía trước viewpoint
        if (aimTarget != null)
        {
            aimTarget.position = viewPoint.position + viewPoint.forward * 5f;
        }
    }

    void UpdateAimTargetPosition()
    {
        if (aimTarget == null || playerCamera == null) return;

        // Raycast từ center màn hình
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPosition;

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimLayerMask))
        {
            // Hit something
            targetPosition = hit.point;
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
        else
        {
            // No hit
            targetPosition = ray.origin + ray.direction * defaultAimDistance;
            Debug.DrawLine(ray.origin, targetPosition, Color.red);
        }

        // Update aim target position
        aimTarget.position = targetPosition;
    }
}
