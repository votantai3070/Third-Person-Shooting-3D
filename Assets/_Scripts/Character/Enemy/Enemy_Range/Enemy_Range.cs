using UnityEngine;

public class Enemy_Range : Enemy
{
    private float lastShootTime;

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
    protected override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public bool CanShoot()
    {
        if (Time.time >= lastShootTime + 1 / visuals.GetCurrentWeaponModel().weaponData.fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    public void ShootPlayer()
    {
        WeaponModels currentModel = visuals.GetCurrentWeaponModel();
        if (currentModel == null)
            return;


        GameObject bullet = ObjectPool.instance.GetObject(
               currentModel.weaponData.bulletPrefab);
        bullet.transform.position = currentModel.gunPoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, 90);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (player.transform.position + Vector3.up * 1.5f - currentModel.gunPoint.transform.position).normalized;
        bulletRb.linearVelocity = direction * currentModel.weaponData.bulletSpeed;
    }
}
