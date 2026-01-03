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

        if (stateTimer < 0 && !isInteractable)
        {
            enemy.ragdoll.CollidersActive(false);
            enemy.ragdoll.RagdollActive(false);
            isInteractable = true;
        }
    }
}
