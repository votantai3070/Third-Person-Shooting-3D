public class Enemy_Melee : Enemy
{
    public IdleState_Melee idleState_Melee { get; private set; }
    public MoveState_Melee patrolState_Melee { get; private set; }
    public ChaseState_Melee chaseState_Melee { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState_Melee = new IdleState_Melee(this, stateMachine, "Idle");
        patrolState_Melee = new MoveState_Melee(this, stateMachine, "Walk");
        chaseState_Melee = new ChaseState_Melee(this, stateMachine, "Chase");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState_Melee);
    }

    protected override void Update()
    {
        base.Update();
    }
}
