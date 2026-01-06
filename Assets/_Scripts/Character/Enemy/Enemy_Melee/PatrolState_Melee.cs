public class PatrolState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    public PatrolState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = false;

        enemy.agent.speed = enemy.moveSpeed;

        enemy.agent.SetDestination(enemy.GetMovePatrol());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.agent.steeringTarget);

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + .5f)
            stateMachine.ChangeState(enemy.idleState);
        else if (enemy.RangeDetectedPlayer() || enemy.isShooted)
            stateMachine.ChangeState(enemy.recoveryState);
    }
}
