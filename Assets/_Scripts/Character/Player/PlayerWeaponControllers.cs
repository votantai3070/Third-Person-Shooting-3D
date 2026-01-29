using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponControllers : MonoBehaviour
{
    public Player player { private set; get; }
    public PlayerControls controls { private set; get; }
    [SerializeField] AmmoInventoryUI ammoInventory;
    private int totalAmmoCurrentWeapon;

    [Header("Elements")]
    private float averageMass;
    private bool weaponReady;
    private bool isShooting;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float distanceShot = 1000f;
    private Transform gunPoint;

    WeaponModels currentWeaponModel;
    [SerializeField] List<Weapon_Data> defaultWeaponData;
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] List<Weapon> weaponSlots;

    [SerializeField] GameObject pickupWeaponPrefab;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        AssignInputEvents();
    }

    private void Update()
    {
        if (isShooting && player.aim.IsAiming())
            Shoot();
    }

    //Add ammo into inventory
    void AddAmmoFromWeaponGenerateIntoInventory()
    {
        foreach (var weapon in weaponSlots)
        {
            ammoInventory.AddAmmo(weapon.weaponType, weapon.totalReserveAmmo);
        }
    }

    #region Equip/Drop/Reload Weapon
    public void SetDefaultWeapon(List<Weapon_Data> newWeaponData)
    {
        defaultWeaponData = new(newWeaponData);
        weaponSlots.Clear();

        foreach (var weaponData in defaultWeaponData)
        {
            Weapon newWeapon = new(weaponData);
            weaponSlots.Add(newWeapon);
        }

        AddAmmoFromWeaponGenerateIntoInventory();

        EquipWeapon(0);
    }

    private void EquipWeapon(int i)
    {
        if (currentWeapon.weaponType == weaponSlots[i].weaponType)
            return;

        if (i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];

        player.visuals.SwitchOffWeaponHolder();
        currentWeaponModel = player.visuals.SwitchOnWeaponHolder();

        if (player.visuals != null)
            player.visuals.PlayWeaponEquipAnimation();

        player.visuals.SwitchAnimationLayer();

        SetupWeapon();

        UpdateAmmoUI();

        SetWeaponReady(true);
    }

    private void UpdateAmmoUI()
    {
        totalAmmoCurrentWeapon = ammoInventory.GetAmmoByType(currentWeapon.bulletPrefab.name);

        UI.instance.weaponSlotUI.UpdateWeaponUI(currentWeapon.weaponData.weaponIcon, currentWeapon.bulletsInMagazine, totalAmmoCurrentWeapon);
    }

    private void DropWeapon()
    {
        if (HasOneWeapon()) return;

        CreateWeaponInGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    private void CreateWeaponInGround()
    {
        GameObject dropped = ObjectPool.instance.GetObject(pickupWeaponPrefab);
        PickupWeapon dropWeapon = dropped.GetComponent<PickupWeapon>();

        // Clone weapon và set ammo = 0
        Weapon droppedWeapon = CloneWeaponWithoutAmmo(currentWeapon);

        dropWeapon.SetupPickupWeapon(transform, droppedWeapon);
    }

    // Method tạo clone weapon với ammo = 0
    private Weapon CloneWeaponWithoutAmmo(Weapon original)
    {
        Weapon clone = new(original.weaponData);

        clone.bulletsInMagazine = original.bulletsInMagazine;

        clone.totalReserveAmmo = 0;

        return clone;
    }

    IEnumerator ReloadWeapon()
    {
        SetWeaponReady(false);

        player.visuals.PlayReloadAnimation();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.RefillBullets();

        UpdateAmmoUI();

        SetWeaponReady(true);
    }

    private bool HasOneWeapon() => weaponSlots.Count <= 1;

    public bool OnlyTwoWeaponInSlotEquip() => weaponSlots.Count < 2 && weaponSlots.Count > 0;
    #endregion

    void SetupWeapon()
    {
        bulletSpeed = currentWeapon.bulletSpeed;
        bulletPrefab = currentWeapon.bulletPrefab;
        averageMass = currentWeapon.impactForce;
        gunPoint = currentWeaponModel.gunPoint;
        distanceShot = currentWeapon.weaponData.shootRange;
    }

    #region Shoot
    private void Shoot()
    {
        if (!WeaponReady()) return;

        if (!currentWeapon.CanShoot()) return;

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;

        player.visuals.PlayFireAnimation();

        if (currentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }


        FireSingleBullet();

        currentWeapon.bulletsInMagazine--;

        UpdateAmmoUI();
    }

    IEnumerator BurstFire()
    {
        SetWeaponReady(false);

        for (int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletsPerShot)
                SetWeaponReady(true);
        }

        currentWeapon.bulletsInMagazine--;

        UpdateAmmoUI();
    }

    public void FireSingleBullet()
    {

        // Raycast từ center màn hình để tìm điểm bắn
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // center screen
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, distanceShot)) // 100f là range tối đa
        {
            targetPoint = hit.point; // Nếu raycast hit object
        }
        else
        {
            targetPoint = ray.GetPoint(distanceShot); // Nếu không hit, lấy điểm xa về phía trước
        }

        gunPoint = currentWeaponModel.gunPoint;

        // Tính direction từ gunPoint đến targetPoint
        Vector3 direction = (targetPoint - gunPoint.position).normalized;

        // Spread
        Vector3 bulletDirection = currentWeapon.ApplySpread(direction);

        // Spawn bullet và bắn
        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, null);

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(bulletDirection) * Quaternion.Euler(90, 0, 0);

        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.SetupBullet(distanceShot, currentWeapon.weaponData);

        Rigidbody rbBullet = newBullet.GetComponent<Rigidbody>();
        rbBullet.mass = averageMass / bulletSpeed;
        rbBullet.linearVelocity = bulletDirection * bulletSpeed;
    }
    #endregion

    public List<Weapon> GetListWeapon() => weaponSlots;

    public void SetWeaponReady(bool ready) => weaponReady = ready;

    public bool WeaponReady() => weaponReady;

    public Weapon CurrentWeapon() => currentWeapon;

    void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Fire.performed += ctx => isShooting = true;
        controls.Player.Fire.canceled += ctx => isShooting = false;

        controls.Player.BurstMode.performed += ctx =>
        {
            SetWeaponReady(true);
            currentWeapon.ToggleBurst();
            Debug.Log("burstActive: " + currentWeapon.burstActive);
        };

        controls.Player.Reload.performed += ctx =>
        {
            if (WeaponReady() && currentWeapon.IsReloading())
                StartCoroutine(ReloadWeapon());
        };

        controls.Player.Drop.performed += ctx => DropWeapon();

        controls.Player.Equip1.performed += ctx => EquipWeapon(0);
        controls.Player.Equip2.performed += ctx => EquipWeapon(1);

        controls.Player.Aim.performed += ctx =>
        {
            if (player.isDead) return;
            player.aim.SetAiming(true);
        };

        controls.Player.Aim.canceled += ctx => player.aim.SetAiming(false);
    }
}
