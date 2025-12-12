using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private WeaponModels[] weaponModels;

    private Transform characterModel;

    public Player player { get; private set; }
    public Transform aim;

    private bool isRunning;

    public Rig rig { get; private set; }
    [SerializeField] float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;

    [Header("Left Hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    public TwoBoneIKConstraint leftHandIK;
    public Transform leftHandElbow;
    bool shouldIncrease_LeftHandWeight;
    [SerializeField] private Transform leftHandIK_Target;

    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;

    private WeaponModels currentWeaponModel;

    private void Awake()
    {
        player = GetComponent<Player>();

        rig = GetComponentInChildren<Rig>();

        weaponModels = GetComponentsInChildren<WeaponModels>(true);

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Start()
    {
        if (player.anim != null)
        {
            characterModel = player.anim.transform;
        }

        currentWeaponModel = GetCurrentWeaponModel();

        AttachLeftHand();
        SwitchAnimationLayer();
    }

    private void Update()
    {
        Debug.Log("shouldIncrease_RigWeight: " + shouldIncrease_RigWeight);
        Debug.Log("shouldIncrease_LeftHandWeight: " + shouldIncrease_LeftHandWeight);

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void LateUpdate()
    {
        UpdateLeftHandIK();
    }

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = GetCurrentWeaponModel().equipType;
        float equipmentSpeed = player.controller.CurrentWeapon().equipmentSpeed;

        ReduceLeftHandIKWeight();
        ReduceRigWeight();

        player.anim.SetFloat("EquipType", ((float)equipType));
        player.anim.SetFloat("EquipSpeed", equipmentSpeed);
        player.anim.SetTrigger("EquipWeapon");
    }


    #region Animation Rigging Methods
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1f)
            {
                leftHandIK.weight = 1f;
                shouldIncrease_LeftHandWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1f)
            {
                rig.weight = 1f;
                shouldIncrease_RigWeight = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = .15f;
    }

    private void ReduceLeftHandIKWeight()
    {
        leftHandIK.weight = 0f;
    }

    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;

    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandWeight = true;
    #endregion

    public Transform GetPlayerViewPointTransform()
    {
        if (currentWeaponModel == null)
            currentWeaponModel = GetCurrentWeaponModel();

        return currentWeaponModel?.playerViewPointTransform;
    }

    public void SetRunning(Vector3 worldDirection)
    {
        Vector3 localDirection = Vector3.zero;

        if (characterModel != null && worldDirection.magnitude > 0.01f)
        {
            localDirection = characterModel.InverseTransformDirection(worldDirection);
        }

        isRunning = worldDirection.magnitude > 0.01f;

        bool isShootingRifle = player.anim.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Weapon");
        bool isShootingPistol = player.anim.GetCurrentAnimatorStateInfo(2).IsName("Shoot_Weapon");
        bool isReloadingRifle = player.anim.GetCurrentAnimatorStateInfo(1).IsName("Reload_Weapon");
        bool isReloadingPistol = player.anim.GetCurrentAnimatorStateInfo(2).IsName("Reload_Weapon");

        bool isShooting = isShootingRifle || isShootingPistol || isReloadingRifle || isReloadingPistol;

        player.anim.SetBool("Running", false);
        player.anim.SetBool("RunAndShoot", false);

        if (isRunning && isShooting)
        {
            player.anim.SetBool("RunAndShoot", true);
        }
        else if (isRunning)
        {
            player.anim.SetBool("Running", true);
        }


        player.anim.SetFloat("x", localDirection.x, 0.1f, Time.deltaTime);
        player.anim.SetFloat("z", localDirection.z, 0.1f, Time.deltaTime);

        if (isRunning)
        {
            ReduceRigWeight();
            ReduceLeftHandIKWeight();
        }
        else if (!isShooting)
        {
            MaximizeRigWeight();
            MaximizeLeftHandWeight();
        }
    }

    public void PlayFireAnimation() => player.anim.SetTrigger("Shooting");

    public void PlayReloadAnimation()
    {
        float reloadSpeed = player.controller.CurrentWeapon().reloadSpeed;

        player.anim.SetTrigger("Reloading");
        player.anim.SetFloat("ReloadSpeed", reloadSpeed);
        ReduceRigWeight();
    }

    private void AttachLeftHand()
    {
        if (currentWeaponModel == null)
        {
            Debug.LogWarning("No current weapon!");
            return;
        }

        if (currentWeaponModel.leftHandIK == null)
        {
            Debug.LogWarning($"Weapon {currentWeaponModel.name} has no leftHandIK!");
            return;
        }

        if (leftHandIK != null && currentWeaponModel.leftHandIK != null)
        {
            leftHandIK_Target.localPosition = currentWeaponModel.leftHandIK.localPosition;
            leftHandIK_Target.localRotation = currentWeaponModel.leftHandIK.localRotation;
        }

        if (leftHandElbow != null && currentWeaponModel.leftHandElbow != null)
        {
            leftHandElbow.localPosition = currentWeaponModel.leftHandElbow.localPosition;
            leftHandElbow.localRotation = currentWeaponModel.leftHandElbow.localRotation;
        }
    }

    void UpdateLeftHandIK()
    {
        if (currentWeaponModel == null || !currentWeaponModel.gameObject.activeInHierarchy)
            return;

        if (leftHandIK != null && currentWeaponModel.leftHandIK != null)
        {
            leftHandIK.transform.localPosition = currentWeaponModel.leftHandIK.localPosition;
            leftHandIK.transform.localRotation = currentWeaponModel.leftHandIK.localRotation;
        }

        //if (leftHandElbow != null && currentWeaponModel.leftHandElbow != null)
        //{
        //    leftHandElbow.localPosition = currentWeaponModel.leftHandElbow.localPosition;
        //    leftHandElbow.localRotation = currentWeaponModel.leftHandElbow.localRotation;
        //}
    }

    private void SwitchAnimationLayer()
    {
        if (currentWeaponModel == null)
            return;

        int layerIndex = (int)currentWeaponModel.layerAnimationType;

        for (int i = 0; i < player.anim.layerCount; i++)
        {
            player.anim.SetLayerWeight(i, 0);
        }

        if (layerIndex < player.anim.layerCount)
        {
            player.anim.SetLayerWeight(layerIndex, 1);
        }
    }

    public WeaponModels GetCurrentWeaponModel()
    {
        WeaponType weaponType = default;

        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);

            // Thêm null check
            if (player.controller != null && player.controller.CurrentWeapon() != null)
                weaponType = player.controller.CurrentWeapon().weaponType;
            else
                continue; // Bỏ qua nếu chưa có weapon

            if (weaponModel.weaponModelType == weaponType)
            {
                weaponModel.gameObject.SetActive(true);
                currentWeaponModel = weaponModel;
                return weaponModel;
            }
        }

        return null;
    }

}
