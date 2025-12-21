public class Enemy_Melee : Enemy
{
    public IdleState_Melee idleState_Melee { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState_Melee = new IdleState_Melee(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState_Melee);
    }


}
