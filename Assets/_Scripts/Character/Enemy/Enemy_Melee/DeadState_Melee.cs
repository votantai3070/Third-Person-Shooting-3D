public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    bool isInteractable;

    public DeadState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
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

        if (stateTimer < 0 && !isInteractable)
        {
            enemy.ragdoll.CollidersActive(false);
            enemy.ragdoll.RagdollActive(false);
            isInteractable = true;
        }
    }
}
