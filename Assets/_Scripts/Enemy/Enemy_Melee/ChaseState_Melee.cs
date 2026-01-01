public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    public ChaseState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = false;

        enemy.agent.speed = enemy.chaseSpeed;

        enemy.agent.stoppingDistance = 2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.player.transform.position);

        enemy.agent.destination = enemy.player.transform.position;

        if (enemy.RangeDetectedAttackPlayer())
            stateMachine.ChangeState(enemy.attackState);
    }
}
