public class Enemy_Range : Enemy
{
    public IdleState_Range idleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
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
