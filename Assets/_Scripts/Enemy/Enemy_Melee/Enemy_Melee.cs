public class Enemy_Melee : Enemy
{
    public IdleState_Melee idleState { get; private set; }
    public PatrolState_Melee patrolState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        patrolState = new PatrolState_Melee(this, stateMachine, "Patrol");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
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
}
