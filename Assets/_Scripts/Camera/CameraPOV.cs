using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    Player player;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float transitionSpeed = 5f;

    private Vector3 normalOffset = new Vector3(0f, 2f, -2f);
    private Vector3 aimOffset = new Vector3(0.6f, 1.25f, 2.33f);

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private bool invertY = false;

    [Header("Vertical Limits")]
    [SerializeField] private float minVerticalAngle = -30f; // Nhìn xuống
    [SerializeField] private float maxVerticalAngle = 70f;  // Nhìn lên

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
        }

        // Set initial shoulder offset
        if (virtualCamera.TryGetComponent<CinemachineThirdPersonFollow>(out var position))
        {
            position.ShoulderOffset = normalOffset;
        }

        // Initialize rotations
        if (cameraTarget != null)
        {
            horizontalRotation = cameraTarget.eulerAngles.y;
            verticalRotation = cameraTarget.eulerAngles.x;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCameraPosition();
        UpdateCameraRotation();
        HandleCursorToggle();
    }

    void UpdateCameraPosition()
    {
        Vector3 targetOffset = player.aim.IsAiming() ? aimOffset : normalOffset;

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

        // ========== HORIZONTAL (Trái/Phải) ==========
        horizontalRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;

        // ========== VERTICAL (Lên/Xuống) ==========
        float verticalInput = mouseDelta.y * mouseSensitivity * Time.deltaTime;
        if (invertY) verticalInput = -verticalInput;

        verticalRotation -= verticalInput;

        // Clamp vertical rotation
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        // Apply rotation to camera target
        cameraTarget.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = locked;
        }
    }

    // Debug visualization
    private void OnGUI()
    {
        if (cameraTarget == null) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 10, 300, 30),
            $"Horizontal: {horizontalRotation:F1}°", style);
        GUI.Label(new Rect(10, 35, 300, 30),
            $"Vertical: {verticalRotation:F1}°", style);
        GUI.Label(new Rect(10, 60, 300, 30),
            $"Camera Rotation: {cameraTarget.eulerAngles}", style);
    }
}
