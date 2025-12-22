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

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.agent.SetDestination(enemy.player.transform.position);

        enemy.RotateFace(enemy.player.transform.position);
    }
}
