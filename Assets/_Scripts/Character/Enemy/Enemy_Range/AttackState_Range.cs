using UnityEngine;

public class AttackState_Range : EnemyState
{
    private Enemy_Range enemy;

    public AttackState_Range(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.SetLayerAnimation(1, 0f);
        enemy.SetupWeapon();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

    }

    public override void Exit()
    {
        base.Exit();

        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.player.transform.position);

        if (!enemy.RangeDetectedAttackPlayer())
        {
            stateMachine.ChangeState(enemy.chaseState);
            return;
        }

        if (enemy.CanShoot() && enemy.isAttack)
        {
            enemy.FireSingleBullet();
        }
    }
}
