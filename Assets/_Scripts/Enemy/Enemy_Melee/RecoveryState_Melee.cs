using UnityEngine;

public class RecoveryState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public RecoveryState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.recoveryTime -= Time.deltaTime;

        enemy.RotateFace(enemy.player.transform.position);

        if (enemy.recoveryTime <= 0)
            stateMachine.ChangeState(enemy.chaseState);
    }
}
