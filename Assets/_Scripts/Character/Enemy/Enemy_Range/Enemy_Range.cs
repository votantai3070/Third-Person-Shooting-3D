using UnityEngine;

public class Enemy_Range : Enemy
{
    private float lastShootTime;
    public bool isAttack = true;

    [Header("Weapon Settings")]
    private Transform gunPoint;
    private float bulletSpeed;
    private Vector3 bulletDirection;
    private float fireRate;

    public GameObject bulletPrefab;

    #region States
    public IdleState_Range idleState { get; private set; }
    public PatrolState_Range patrolState { get; private set; }
    public RecoveryState_Range recoveryState { get; private set; }
    public ChaseState_Range chaseState { get; private set; }
    public AttackState_Range attackState { get; private set; }
    public DeadState_Range deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        patrolState = new PatrolState_Range(this, stateMachine, "Patrol");
        recoveryState = new RecoveryState_Range(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Range(this, stateMachine, "Chase");
        attackState = new AttackState_Range(this, stateMachine, "Attack");
        deadState = new DeadState_Range(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SetupWeapon()
    {
        EnemyWeaponModel_Range weaponModel = visuals.currentWeaponModel.GetComponent<EnemyWeaponModel_Range>();

        gunPoint = weaponModel.gunPoint;
        bulletSpeed = weaponModel.weaponData.bulletSpeed;
        fireRate = weaponModel.weaponData.fireRate;
        bulletPrefab = weaponModel.weaponData.bulletPrefab;
    }

    public void FireSingleBullet()
    {
        //anim.SetTrigger("Shoot");

        bulletDirection = (player.transform.position - gunPoint.position).normalized;

        Bullet newBullet = ObjectPool.instance.GetObject(bulletPrefab, null).GetComponent<Bullet>();
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(bulletDirection) * Quaternion.Euler(90, 0, 0);

        Weapon_Data weaponData = visuals.currentWeaponModel.GetComponent<EnemyWeaponModel_Range>().weaponData;

        newBullet.SetupBullet(weaponData.shootRange, weaponData);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        //Vector3 bulletDirectionWithSpread = currentModel.weaponData.ApplyWeaponSpread(bulletDirection);

        rbNewBullet.mass = 20 / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }
    protected override void Die()
    {
        base.Die();

        if (dropController.missionObjectKey != null)
            dropController.DropItem();

        GetComponent<MissionObject_Hunt>()?.InvokeTargetKilled();

        stateMachine.ChangeState(deadState);
    }

    public bool CanShoot()
    {
        if (Time.time >= lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

}
