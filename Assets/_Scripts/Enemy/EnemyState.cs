public class EnemyState
{
    protected Enemy enemyBase;
    protected StateMachine stateMachine;

    protected string animBoolName;

    public EnemyState(Enemy enemyBase, StateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName, true);

    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }

    public virtual void Update() { }
}
