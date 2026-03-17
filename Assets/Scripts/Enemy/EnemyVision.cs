using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Vision")]
    public float viewDistance = 8f;
    [Range(0, 360)]
    public float viewAngle = 120f;

    [Header("Attack")]
    public float attackDistance = 1.6f;

    [Header("Mask")]
    public LayerMask obstacleMask;

    [Header("Refs")]
    public Transform eye;

    private bool wasSeeingTarget = false;

    void Awake()
    {
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
    }

    void Update()
    {
        bool canSee = CanSeeTarget();

        if (canSee && !wasSeeingTarget)
        {
            if (BGMManager.Instance != null)
                BGMManager.Instance.AddChaser();
        }
        else if (!canSee && wasSeeingTarget)
        {
            if (BGMManager.Instance != null)
                BGMManager.Instance.RemoveChaser();
        }

        wasSeeingTarget = canSee;
    }

    void OnDisable()
    {
        if (wasSeeingTarget && BGMManager.Instance != null)
        {
            BGMManager.Instance.RemoveChaser();
            wasSeeingTarget = false;
        }
    }

    public bool CanSeeTarget()
    {
        if (target == null) return false;

        Vector3 origin = eye ? eye.position : transform.position + Vector3.up * 1.5f;

        Vector3 targetPos = target.position + Vector3.up * 1.0f;
        Vector3 dir = targetPos - origin;

        Debug.DrawRay(origin, dir.normalized * viewDistance, Color.red);

        float dist = dir.magnitude;
        if (dist > viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(origin, dir.normalized, dist, obstacleMask, QueryTriggerInteraction.Ignore))
            return false;

        return true;
    }

    public bool InAttackRange()
    {
        if (target == null) return false;

        Vector3 a = transform.position;
        Vector3 b = target.position;
        a.y = 0;
        b.y = 0;

        return Vector3.Distance(a, b) <= attackDistance;
    }
}