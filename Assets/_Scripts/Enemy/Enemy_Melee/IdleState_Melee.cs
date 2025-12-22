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

        stateTimer = enemy.idleTimer;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.patrolState_Melee);
        else if (enemy.ChasePlayer())
            stateMachine.ChangeState(enemy.chaseState_Melee);
    }
}
