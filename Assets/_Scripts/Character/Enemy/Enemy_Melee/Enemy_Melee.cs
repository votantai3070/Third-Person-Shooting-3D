using UnityEngine;

public class Enemy_Melee : Enemy
{
    public Enemy_MeleeSFX meleeSFX { get; private set; }

    #region States
    public IdleState_Melee idleState { get; private set; }
    public PatrolState_Melee patrolState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
    #endregion

    public int damaged = 10;

    [Header("Zombie Enemy Info")]
    public CapsuleCollider armLeftZombieCollider;
    public CapsuleCollider armRightZombieCollider;

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        patrolState = new PatrolState_Melee(this, stateMachine, "Patrol");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        deadState = new DeadState_Melee(this, stateMachine, "Idle");

        meleeSFX = GetComponent<Enemy_MeleeSFX>();
    }

    protected override void Start()
    {
        base.Start();

        DisabledTrailRenderer();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Die()
    {
        if (dropController.missionObjectKey != null)
            dropController.DropItem();

        GetComponent<MissionObject_Hunt>()?.InvokeTargetKilled();

        stateMachine.ChangeState(deadState);
    }

    public void EnableColliderEnemyZombie(bool enabled)
    {
        if (armLeftZombieCollider != null)
            armLeftZombieCollider.enabled = enabled;

        if (armRightZombieCollider != null)
            armRightZombieCollider.enabled = enabled;
    }
}
