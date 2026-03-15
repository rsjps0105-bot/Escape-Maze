using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyMotor))]
[RequireComponent(typeof(EnemyVision))]
public class Enemy : MonoBehaviour
{
    // Components
    public Animator Anim { get; private set; }
    public EnemyMotor Motor { get; private set; }
    public EnemyVision Vision { get; private set; }

    // Animator Params
    public readonly int SpeedParam = Animator.StringToHash("Speed");
    public readonly int AttackParam = Animator.StringToHash("Attack");

    // Attack Settings
    [Header("Attack")]
    public float attackCooldown = 1.0f;
    [HideInInspector] public float nextAttackTime;
    public float attackDamage = 100f;
    public float attackRadius = 0.7f;
    public float attackDistance = 1.2f;
    public LayerMask attackMask;
    public Transform attackOrigin;

    [Header("Patrol")]
    public PatrolAxis patrolAxis = PatrolAxis.Horizontal;
    public LayerMask obstacleMask;
    public float wallCheckDistance = 0.6f;

    [HideInInspector] public int patrolDirection = 1;

    // StateMachine
    public EnemyStateMachine SM { get; private set; }

    public EnemyLocomotionState LocomotionState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    void Awake()
    {
        Anim = GetComponent<Animator>();
        Motor = GetComponent<EnemyMotor>();
        Vision = GetComponent<EnemyVision>();

        SM = new EnemyStateMachine();
        LocomotionState = new EnemyLocomotionState(this);
        AttackState = new EnemyAttackState(this);
    }

    void Start()
    {
        SM.ChangeState(LocomotionState);
    }

    void Update()
    {
        SM.Tick();
    }

    public void ReversePatrol()
    {
        patrolDirection *= -1;
    }

    public Vector3 GetPatrolDirection()
    {
        if (patrolAxis == PatrolAxis.Horizontal)
        {
            // â°à⁄ìÆ: xï˚å¸
            return Vector3.right * patrolDirection;
        }
        else
        {
            // ècà⁄ìÆ: zï˚å¸
            return Vector3.forward * patrolDirection;
        }
    }

    // AnimEvent Ç©ÇÁåƒÇŒÇÍÇÈ
    public void AnimEvent_AttackHit()
    {
        Transform origin = attackOrigin != null ? attackOrigin : transform;
        Vector3 center = origin.position + origin.forward * attackDistance;

        Collider[] hits = Physics.OverlapSphere(
            center,
            attackRadius,
            attackMask,
            QueryTriggerInteraction.Ignore);

        foreach (var col in hits)
        {
            var hp = col.GetComponentInParent<Health>();

            if (hp != null)
            {
                hp.TakeDamage(attackDamage);
                Debug.Log($"{name} hit {col.name}");
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Transform origin = attackOrigin != null ? attackOrigin : transform;
        Vector3 center = origin.position + origin.forward * attackDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, attackRadius);

        // èÑâÒï˚å¸ÇÃämîFóp
        Gizmos.color = Color.yellow;
        Vector3 dir = Application.isPlaying ? GetPatrolDirection() :
            (patrolAxis == PatrolAxis.Horizontal ? Vector3.right : Vector3.forward);
        Gizmos.DrawLine(transform.position, transform.position + dir.normalized * wallCheckDistance);
    }
}

public enum PatrolAxis
{
    Horizontal, // â°(x)
    Vertical    // èc(z)
}