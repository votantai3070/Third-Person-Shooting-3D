public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    float lastUpdateTime;
    public ChaseState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = false;

        enemy.agent.speed = enemy.chaseSpeed;

        enemy.agent.stoppingDistance = 1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.player.transform.position);

        //if (CanUpdateDestination())
        enemy.agent.destination = enemy.player.transform.position;

        if (enemy.RangeDetectedAttackPlayer())
            stateMachine.ChangeState(enemy.attackState);
    }

    //private bool CanUpdateDestination()
    //{
    //    if (Time.time > lastUpdateTime + .25f)
    //    {
    //        lastUpdateTime = Time.time;
    //        return true;
    //    }
    //    return false;
    //}
}
