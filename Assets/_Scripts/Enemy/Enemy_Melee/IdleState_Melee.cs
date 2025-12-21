public class IdleState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public IdleState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetFloat("IdleIndex", (int)enemy.visuals.idleType);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
