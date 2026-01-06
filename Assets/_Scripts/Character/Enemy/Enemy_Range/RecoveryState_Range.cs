using UnityEngine;

public class RecoveryState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float recoveryTime;

    public RecoveryState_Range(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.SetLayerAnimation(1, 0f);

        enemy.agent.isStopped = true;

        recoveryTime = enemy.recoveryTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.player.transform.position);

        recoveryTime -= Time.deltaTime;

        if (recoveryTime < 0)
        {
            if (enemy.RangeDetectedPlayer())
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
            else
            {
                stateMachine.ChangeState(enemy.patrolState);
            }
        }
    }
}
