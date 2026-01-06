using UnityEngine;

public class ChaseState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float updateInterval = 0.2f; // Update mỗi 0.2s thay vì mỗi frame
    private float lastUpdateTime;

    public ChaseState_Range(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.SetLayerAnimation(1, 0f);

        enemy.agent.isStopped = false;

        enemy.agent.stoppingDistance = 5f;

        lastUpdateTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.player.transform.position);

        if (Time.time - lastUpdateTime >= updateInterval)
        {
            enemy.agent.SetDestination(enemy.player.transform.position);
            lastUpdateTime = Time.time;
        }

        if (!enemy.RangeDetectedPlayer())
        {
            stateMachine.ChangeState(enemy.recoveryState);
        }
    }
}
