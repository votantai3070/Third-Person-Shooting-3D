using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    Player player;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float transitionSpeed = 5f;

    private Vector3 playerOffset = new Vector3(0f, 2f, -2f);
    private Vector3 aimOffset = new Vector3(0.6f, 1.25f, 2.33f);
    private Vector3 carOffset = new Vector3(0, 5, 0);

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private bool invertY = false;

    [Header("Vertical Limits")]
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 70f;

    // ========== ZOOM SETTINGS (MỚI) ==========
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSensitivity = 1f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float zoomSpeed = 10f;

    private float targetDistance;
    private float currentDistance;
    private float defaultDistance; // Lưu distance mặc định
    // ==========================================

    private CinemachineOrbitalFollow orbitalFollow;
    private CinemachineThirdPersonFollow thirdPersonFollow;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    private void Start()
    {
        player = GetComponentInParent<Player>();

        if (virtualCamera == null)
        {
            virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        }

        if (virtualCamera != null)
        {
            orbitalFollow = virtualCamera.GetComponent<CinemachineOrbitalFollow>();
            thirdPersonFollow = virtualCamera.GetComponent<CinemachineThirdPersonFollow>();

            if (cameraTarget == null)
            {
                cameraTarget = virtualCamera.Follow;
            }

            // ========== INITIALIZE ZOOM (MỚI) ==========
            if (thirdPersonFollow != null)
            {
                defaultDistance = thirdPersonFollow.CameraDistance;
                currentDistance = defaultDistance;
                targetDistance = defaultDistance;
            }
            // ===========================================
        }

        // Set initial shoulder offset
        if (virtualCamera.TryGetComponent<CinemachineThirdPersonFollow>(out var position))
        {
            if (GameManager.instance.isPlayerView)
                position.ShoulderOffset = playerOffset;
            else
                position.ShoulderOffset = carOffset;
        }

        // Initialize rotations
        if (cameraTarget != null)
        {
            horizontalRotation = cameraTarget.eulerAngles.y;
            verticalRotation = cameraTarget.eulerAngles.x;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!player.controlsEnabled)
        {
            return;
        }

        HandleCursorToggle();
        UpdateCameraPosition();
        UpdateCameraRotation();
        UpdateCameraZoom();
    }

    void UpdateCameraPosition()
    {
        Vector3 targetOffset = player.aim.IsAiming()
            ? aimOffset : GameManager.instance.isPlayerView
            ? playerOffset : carOffset;

        if (virtualCamera.TryGetComponent<CinemachineThirdPersonFollow>(out var position))
        {
            position.ShoulderOffset = Vector3.Lerp(
                position.ShoulderOffset,
                targetOffset,
                Time.deltaTime * transitionSpeed);
        }
    }

    void UpdateCameraRotation()
    {
        if (cameraTarget == null) return;

        Vector2 mouseDelta = InputManager.instance.GetMouseDelta();

        if (mouseDelta.magnitude < 0.001f) return;

        horizontalRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;

        float verticalInput = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        if (invertY) verticalInput = -verticalInput;

        verticalRotation -= verticalInput;

        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        cameraTarget.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    void UpdateCameraZoom()
    {
        if (thirdPersonFollow == null) return;

        if (player.aim.IsAiming()) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            targetDistance -= scrollInput * zoomSensitivity;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        // Smooth zoom transition
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);
        thirdPersonFollow.CameraDistance = currentDistance;
    }

    void HandleCursorToggle()
    {
        if (Input.GetMouseButtonDown(3))
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = locked;
        }
    }

    public void ResetZoom()
    {
        targetDistance = defaultDistance;
    }

    public void SetZoom(float distance)
    {
        targetDistance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    // Debug visualization
    //private void OnGUI()
    //{
    //    if (cameraTarget == null) return;

    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 16;
    //    style.normal.textColor = Color.yellow;

    //    GUI.Label(new Rect(10, 10, 300, 30),
    //        $"Horizontal: {horizontalRotation:F1}°", style);
    //    GUI.Label(new Rect(10, 35, 300, 30),
    //        $"Vertical: {verticalRotation:F1}°", style);
    //    GUI.Label(new Rect(10, 60, 300, 30),
    //        $"Camera Rotation: {cameraTarget.eulerAngles}", style);
    //    
    //    // ========== DEBUG ZOOM (MỚI) ==========
    //    GUI.Label(new Rect(10, 85, 300, 30),
    //        $"Zoom Distance: {currentDistance:F2}", style);
    //    // ======================================
    //}
}
