public class DeadState_Range : EnemyState
{
    private Enemy_Range enemy;

    bool isInteractable;

    public DeadState_Range(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;

        enemy.anim.enabled = false;

        isInteractable = false;

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0 && !isInteractable)
        {
            enemy.ragdoll.RagdollActive(false);
            enemy.ragdoll.CollidersActive(false);
        }
    }
}
