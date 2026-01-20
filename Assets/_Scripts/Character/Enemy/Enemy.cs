using UnityEngine;
using UnityEngine.AI;

public enum EnemyAnimationType
{
    OneHand_Melee,
    TwoHand_Melee
}

public class Enemy : Character
{
    public StateMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public EnemyVisuals visuals { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Player player { get; private set; }
    public Enemy_Ragdoll ragdoll { get; private set; }
    public Enemy_DropController dropController { get; private set; }

    [Header("General Settings")]
    public float idleTimer;
    public float moveSpeed = 3.5f;
    public float turnSpeed = 5f;
    public float chaseRange = 10f;
    public float chaseSpeed = 5f;
    public float attackRange = 2f;
    public float attackCooldown;
    private float lastAttackTime;


    public bool isTrigger;
    public bool isShooted;

    [Header("Patrol Settings")]
    public int currentPatrolIndex;
    [SerializeField] PointPatrol[] pointPatrols;
    private Vector3[] patrolPointPosition;

    [Header("Recovery Settings")]
    public float recoveryTime = 1f;



    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
    }
    protected virtual void Start()
    {
        pointPatrols = GetComponentsInChildren<PointPatrol>();
        anim = GetComponentInChildren<Animator>();
        visuals = GetComponent<EnemyVisuals>();
        agent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<Player>();
        ragdoll = GetComponent<Enemy_Ragdoll>();
        dropController = GetComponent<Enemy_DropController>();

        IniatializePatrolPoints();
    }

    protected virtual void Update()
    {
        stateMachine.currentState?.Update();
    }

    public void EnemyVIP()
    {
        int healthVIP = Mathf.RoundToInt(GetHealth() * 1.5f);
        Heal(healthVIP);
        transform.localScale = transform.localScale * 1.2f;
    }

    public void Shooted() => isShooted = true;

    public bool IsAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            return true;
        }

        return false;
    }

    public bool RangeDetectedPlayer() => Vector3.Distance(transform.position, player.transform.position) <= chaseRange;

    public bool RangeDetectedAttackPlayer() => Vector3.Distance(transform.position, player.transform.position) <= attackRange;

    public void RotateFace(Vector3 target)
    {
        Vector3 moveDir = (target - transform.position).normalized;
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
    }

    public bool ReachedDestination()
    {
        // Kiểm tra path không đang pending
        if (agent.pathPending)
            return false;

        // Kiểm tra đã đến gần destination
        if (agent.remainingDistance > agent.stoppingDistance + 0.5f)
            return false;

        // Kiểm tra không còn path hoặc velocity = 0
        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            return true;

        return false;
    }

    public Vector3 GetMovePatrol()
    {
        Vector3 patrolPointDestination = patrolPointPosition[currentPatrolIndex];

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPointPosition.Length)
        {
            currentPatrolIndex = 0;
        }

        return patrolPointDestination;
    }

    private void IniatializePatrolPoints()
    {
        patrolPointPosition = new Vector3[pointPatrols.Length];

        for (int i = 0; i < pointPatrols.Length; i++)
        {
            patrolPointPosition[i] = pointPatrols[i].transform.position;
            pointPatrols[i].gameObject.SetActive(false);
        }
    }

    public void DisabledTrailRenderer()
    {
        //visuals.currentMeleeWeaponModel.GetComponentInChildren<TrailRenderer>().enabled = false;
    }

    public void EnabledTrailRenderer()
    {
        //visuals.currentMeleeWeaponModel.GetComponentInChildren<TrailRenderer>().enabled = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

    }
}
