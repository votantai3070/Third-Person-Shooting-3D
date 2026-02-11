using UnityEngine;

public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private float attackTypeIndex;
    private float attackIndex;

    public AttackState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnableColliderEnemyZombie(false);

        attackIndex = attackTypeIndex == 1 ? Random.Range(0, 2) : -1;

        enemy.anim.SetFloat("AttackTypeIndex", attackTypeIndex);
        enemy.anim.SetFloat("AttackIndex", attackIndex);
    }

    public override void Exit()
    {
        base.Exit();

        attackTypeIndex = enemy.RangeDetectedAttackPlayer() ? 1 : 0;
    }

    //private bool PlayerClose()
    //{
    //    return Vector3.Distance
    //        (enemy.transform.position, enemy.player.transform.position) <= 3f;
    //}

    public override void Update()
    {
        base.Update();

        enemy.RotateFace(enemy.player.transform.position);

        if (enemy.IsAttack())
            return;

        if (enemy.isTrigger)
            if (enemy.RangeDetectedAttackPlayer())
                stateMachine.ChangeState(enemy.recoveryState);
            else
                stateMachine.ChangeState(enemy.chaseState);
    }
}
