using UnityEngine;

public class RecoveryState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private float recoveryTime;

    public RecoveryState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;

        this.recoveryTime = enemy.recoveryTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        recoveryTime -= Time.deltaTime;

        enemy.RotateFace(enemy.player.transform.position);

        if (recoveryTime <= 0)
            stateMachine.ChangeState(enemy.chaseState);
    }
}
