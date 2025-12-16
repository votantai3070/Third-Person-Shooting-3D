using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponControllers : MonoBehaviour
{
    public Player player { private set; get; }
    public PlayerControls controls { private set; get; }

    [Header("Elements")]
    private float averageMass;
    private bool weaponReady;
    private bool isShooting;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float distanceShot = 1000f;
    private Transform gunPoint;

    WeaponModels currentWeaponModel;
    [SerializeField] Weapon_SO defaultWeaponData;
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

        EquipStartingWeapon();
    }

    private void Update()
    {
        if (isShooting)
            Shoot();
    }

    #region Equip/Drop/Reload Weapon
    private void EquipStartingWeapon()
    {
        if (weaponSlots.Count == 0)
        {
            weaponSlots.Add(new Weapon(defaultWeaponData));
            //weaponSlots[0] = new Weapon(defaultWeaponData);
        }

        EquipWeapon(0);
    }

    private void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];

        player.visuals.SwitchOffWeaponHolder();
        currentWeaponModel = player.visuals.SwitchOnWeaponHolder();

        if (player.visuals != null)
            player.visuals.PlayWeaponEquipAnimation();

        player.visuals.SwitchAnimationLayer();

        SetupWeapon();

        SetWeaponReady(true);
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
        dropWeapon.SetupPickupWeapon(transform, currentWeapon);
    }

    IEnumerator ReloadWeapon()
    {
        SetWeaponReady(false);

        player.visuals.PlayReloadAnimation();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.RefillBullets();

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
    }

    public void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;

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
        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab);

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(bulletDirection) * Quaternion.Euler(90, 0, 0);

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
    }
}
