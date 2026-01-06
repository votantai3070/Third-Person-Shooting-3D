public class Enemy_Range : Enemy
{
    public IdleState_Range idleState { get; private set; }
    public PatrolState_Range patrolState { get; private set; }
    public RecoveryState_Range recoveryState { get; private set; }
    public ChaseState_Range chaseState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        patrolState = new PatrolState_Range(this, stateMachine, "Patrol");
        recoveryState = new RecoveryState_Range(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Range(this, stateMachine, "Chase");
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

        // stateMachine.ChangeState(deadState);
    }
}
