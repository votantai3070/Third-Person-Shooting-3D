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

        isInteractable = false;
        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;
        enemy.ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        DisableInteractionIfShould();
    }

    private void DisableInteractionIfShould()
    {
        if (stateTimer <= 0 && !isInteractable)
        {
            enemy.ragdoll.RagdollActive(false);
            enemy.ragdoll.CollidersActive(false);
            isInteractable = true;
        }
    }
}
