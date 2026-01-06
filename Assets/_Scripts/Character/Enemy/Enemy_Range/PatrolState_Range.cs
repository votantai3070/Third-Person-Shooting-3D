public class PatrolState_Range : EnemyState
{
    private Enemy_Range enemy;

    public PatrolState_Range(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.ResetPath();

        enemy.visuals.SetLayerAnimation(1, 1f);

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

        if (enemy.ReachedDestination())
            stateMachine.ChangeState(enemy.idleState);

        if (enemy.RangeDetectedPlayer() || enemy.isShooted)
            stateMachine.ChangeState(enemy.recoveryState);
    }
}
