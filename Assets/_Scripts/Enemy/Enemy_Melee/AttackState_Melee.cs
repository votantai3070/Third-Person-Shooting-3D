public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public AttackState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

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
