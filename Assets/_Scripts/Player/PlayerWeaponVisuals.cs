using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private WeaponModelType weaponType;
    [SerializeField] private WeaponModels[] weaponModels;

    private Animator animator;
    private Transform characterModel; // Transform của model (BountyHunterRIO2)

    public Player player { get; private set; }
    public Rig rig { get; private set; }
    public Transform leftHandIK;
    public Transform leftHandElbow;
    public Transform aim;

    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;

    private WeaponModels currentWeapon;
    private int currentLayerIndex = -1;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModels>(true);

        // ========== QUAN TRỌNG: Lấy character model transform ==========
        if (animator != null)
        {
            characterModel = animator.transform;
        }

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Start()
    {
        currentWeapon = GetCurrentWeapon();
        AttachLeftHand();
        SwitchAnimationLayer();
    }

    private void LateUpdate()
    {
        UpdateLeftHandIK();
    }

    public Transform GetPlayerViewPointTransform()
    {
        if (currentWeapon == null)
            currentWeapon = GetCurrentWeapon();

        return currentWeapon?.playerViewPointTransform;
    }

    public void SetRunning(Vector3 worldDirection)
    {
        // ========== FIX: Convert relative to CHARACTER MODEL, không phải Player root ==========
        Vector3 localDirection = Vector3.zero;

        if (characterModel != null && worldDirection.magnitude > 0.01f)
        {
            localDirection = characterModel.InverseTransformDirection(worldDirection);
        }

        bool isRunning = worldDirection.magnitude > 0.01f;

        // Set animator parameters
        animator.SetBool("Running", isRunning);
        animator.SetFloat("x", localDirection.x, 0.1f, Time.deltaTime);
        animator.SetFloat("z", localDirection.z, 0.1f, Time.deltaTime);
    }

    private void AttachLeftHand()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("No current weapon!");
            return;
        }

        if (currentWeapon.leftHandIK == null)
        {
            Debug.LogWarning($"Weapon {currentWeapon.name} has no leftHandIK!");
            return;
        }

        if (leftHandIK != null && currentWeapon.leftHandIK != null)
        {
            leftHandIK.localPosition = currentWeapon.leftHandIK.localPosition;
            leftHandIK.localRotation = currentWeapon.leftHandIK.localRotation;
        }

        if (leftHandElbow != null && currentWeapon.leftHandElbow != null)
        {
            leftHandElbow.localPosition = currentWeapon.leftHandElbow.localPosition;
            leftHandElbow.localRotation = currentWeapon.leftHandElbow.localRotation;
        }
    }

    void UpdateLeftHandIK()
    {
        if (currentWeapon == null || !currentWeapon.gameObject.activeInHierarchy)
            return;

        if (leftHandIK != null && currentWeapon.leftHandIK != null)
        {
            leftHandIK.localPosition = currentWeapon.leftHandIK.localPosition;
            leftHandIK.localRotation = currentWeapon.leftHandIK.localRotation;
        }

        if (leftHandElbow != null && currentWeapon.leftHandElbow != null)
        {
            leftHandElbow.localPosition = currentWeapon.leftHandElbow.localPosition;
            leftHandElbow.localRotation = currentWeapon.leftHandElbow.localRotation;
        }
    }

    private void SwitchAnimationLayer()
    {
        if (currentWeapon == null)
            return;

        int layerIndex = (int)currentWeapon.layerAnimationType;

        if (layerIndex == currentLayerIndex)
            return;

        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        if (layerIndex < animator.layerCount)
        {
            animator.SetLayerWeight(layerIndex, 1);
            currentLayerIndex = layerIndex;
        }
    }

    public WeaponModels GetCurrentWeapon()
    {
        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);

            if (weaponModel.weaponModelType == weaponType)
            {
                weaponModel.gameObject.SetActive(true);
                currentWeapon = weaponModel;
                return weaponModel;
            }
        }

        return null;
    }

    // Debug để check giá trị
    private void OnGUI()
    {
        if (animator == null) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 200, 400, 25), $"Running: {animator.GetBool("Running")}", style);
        GUI.Label(new Rect(10, 225, 400, 25), $"X: {animator.GetFloat("x"):F2}", style);
        GUI.Label(new Rect(10, 250, 400, 25), $"Z: {animator.GetFloat("z"):F2}", style);

        if (characterModel != null)
        {
            GUI.Label(new Rect(10, 275, 400, 25), $"Model Rotation: {characterModel.eulerAngles.y:F1}°", style);
        }
    }
}
