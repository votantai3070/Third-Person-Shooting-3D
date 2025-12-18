using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerControls controls;
    Player player;

    Vector2 moveInput;
    Vector3 moveDirection;
    CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float moveSpeed = 2f;
    private float movementSpeed;
    [SerializeField] private float rotationSpeed = 10f;
    [Range(0f, 1f)] private float runAndShootSpeed;

    [Header("References")]
    [SerializeField] private Transform cameraTarget; // PlayerView
    [SerializeField] private Transform characterModel; // Visual model

    [Header("Aiming")]
    [SerializeField] private bool isAiming = false;

    [Header("Gravity")]
    [SerializeField] private float gravity = 9.81f;

    private float verticalVelocity = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();

        // Auto-find references
        if (cameraTarget == null)
        {
            cameraTarget = transform.Find("PlayerView");
            if (cameraTarget == null)
            {
                // Create PlayerView nếu chưa có
                GameObject pv = new GameObject("PlayerView");
                pv.transform.SetParent(transform);
                pv.transform.localPosition = new Vector3(0, 1.6f, 0);
                cameraTarget = pv.transform;
            }
        }

        if (characterModel == null)
        {
            Animator animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                characterModel = animator.transform;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        movementSpeed = runSpeed;

        AssignInput();
    }

    private void Update()
    {
        ApplyRotation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    void ApplyRotation()
    {
        if (characterModel == null || cameraTarget == null) return;

        if (isAiming)
        {
            // AIM MODE: Xoay theo camera (strafe movement)
            RotateTowardsCamera();
        }
        else
        {
            // NORMAL MODE: Xoay theo hướng di chuyển
            RotateTowardsMovement();
        }
    }

    void RotateTowardsCamera()
    {
        Vector3 cameraForward = cameraTarget.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude < 0.01f) return;

        cameraForward.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        characterModel.rotation = Quaternion.Slerp(
            characterModel.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
    void RotateTowardsMovement()
    {
        if (moveInput.y > 0.1f && moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            characterModel.rotation = Quaternion.Slerp(
                characterModel.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void ApplyMovement()
    {
        if (cameraTarget == null) return;

        // Get camera direction (flatten Y)
        Vector3 cameraForward = cameraTarget.forward;
        Vector3 cameraRight = cameraTarget.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate move direction
        moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        bool isShootingRifle = player.anim.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Weapon");
        bool isShootingPistol = player.anim.GetCurrentAnimatorStateInfo(2).IsName("Shoot_Weapon");
        bool isReloadingRifle = player.anim.GetCurrentAnimatorStateInfo(1).IsName("Reload_Weapon");
        bool isReloadingPistol = player.anim.GetCurrentAnimatorStateInfo(2).IsName("Reload_Weapon");
        bool isEquipWeapon = player.anim.GetCurrentAnimatorStateInfo(1).IsName("Equip_Weapon") ||
            player.anim.GetCurrentAnimatorStateInfo(2).IsName("Equip_Weapon");

        bool isReact = isShootingRifle || isShootingPistol || isReloadingRifle || isReloadingPistol;

        if (isReact)
        {
            runAndShootSpeed = 0.5f;
            movementSpeed = moveSpeed;
        }
        else
        {
            runAndShootSpeed = 1f;
            movementSpeed = runSpeed;
        }

        // Apply movement
        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        // Apply gravity
        ApplyGravity();

        // Update animator
        player.anim.SetFloat("RunAndShootSpeed", runAndShootSpeed);
        player.visuals.SetRunning(moveDirection, isShootingRifle, isShootingPistol, isReloadingRifle, isReloadingPistol, isEquipWeapon);
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void AssignInput()
    {
        controls = player.controls;

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
        Crosshair.instance.SetVisible(aiming);
    }

    public bool IsAiming() => isAiming;

    // Debug
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 14;
        style.normal.textColor = Color.cyan;

        GUI.Label(new Rect(10, 100, 300, 25), $"Move Input: {moveInput}", style);
        GUI.Label(new Rect(10, 120, 300, 25), $"Move Dir: {moveDirection}", style);
        GUI.Label(new Rect(10, 140, 300, 25), $"Is Aiming: {isAiming}", style);
        GUI.Label(new Rect(10, 160, 300, 25), $"Grounded: {characterController.isGrounded}", style);
    }
}
