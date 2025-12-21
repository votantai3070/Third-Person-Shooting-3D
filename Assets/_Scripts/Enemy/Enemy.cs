using UnityEngine;

public enum EnemyIdleType
{
    OneHand_MeleeIdle,
    TwoHand_MeleeIdle
}

public class Enemy : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public EnemyVisuals visuals { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();

        anim = GetComponentInChildren<Animator>();
        visuals = GetComponent<EnemyVisuals>();
    }

    protected virtual void Start()
    {

    }
}
