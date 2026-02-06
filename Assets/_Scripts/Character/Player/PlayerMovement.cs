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


    [Header("Gravity")]
    [SerializeField] private float gravity = 9.81f;

    [Header("Audio")]
    private AudioSource walkSFX;
    private AudioSource runSFX;

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

        if (player.sound.walkSFX != null)
            walkSFX = player.sound.walkSFX;

        if (player.sound.runSFX != null)
            runSFX = player.sound.runSFX;

        AssignInput();
    }

    private void Update()
    {
        if (player.isDead) return;

        ApplyRotation();
    }

    private void FixedUpdate()
    {
        if (player.isDead) return;

        ApplyMovement();
    }

    void ApplyRotation()
    {
        if (characterModel == null || cameraTarget == null) return;

        if (player.aim.IsAiming())
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

        bool isShootingRifle, isShootingPistol, isReloadingRifle, isReloadingPistol, isEquipWeapon, isInteraction;
        player.visuals.GetAnimationState(out isShootingRifle, out isShootingPistol, out isReloadingRifle, out isReloadingPistol, out isEquipWeapon, out isInteraction);

        bool isReact = isShootingRifle || isShootingPistol || isReloadingRifle || isReloadingPistol;

        if (isReact)
        {
            runAndShootSpeed = 0.5f;
            movementSpeed = moveSpeed;
        }
        else if (isInteraction)
        {
            runAndShootSpeed = 0f;
            movementSpeed = 0f;
        }
        else
        {
            runAndShootSpeed = 1f;
            movementSpeed = runSpeed;
        }

        //Apply SFX
        if (movementSpeed == moveSpeed && moveDirection.magnitude > 0)
            SoundWalkFX();
        else if (movementSpeed == runSpeed && moveDirection.magnitude > 0)
            SoundRunFX();
        else
            StopMoveFX();

        // Apply movement
        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        // Apply gravity
        ApplyGravity();

        // Update animator
        player.anim.SetFloat("RunAndShootSpeed", runAndShootSpeed);
        player.visuals.SetRunning(moveDirection, isShootingRifle, isShootingPistol, isReloadingRifle, isReloadingPistol, isEquipWeapon, isInteraction);
    }

    private void SoundRunFX()
    {
        runSFX.Play();
        walkSFX.Stop();
    }

    private void StopMoveFX()
    {
        runSFX.Stop();
        walkSFX.Stop();
    }

    private void SoundWalkFX()
    {
        walkSFX.Play();
        runSFX.Stop();
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
        controls = ControlsManager.instance.controls;

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx =>
        {
            StopMoveFX();
            moveInput = Vector2.zero;
        };
    }
}
