using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMotor : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float turnSpeed = 10f;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void MoveTo(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.01f)
            return;

        RotateTo(dir);

        Vector3 move = dir.normalized * moveSpeed * Time.deltaTime;
        controller.Move(move);
    }

    public void MoveDirection(Vector3 dir)
    {
        dir.y = 0;

        if (dir.sqrMagnitude < 0.01f)
            return;

        RotateTo(dir);

        Vector3 move = dir.normalized * moveSpeed * Time.deltaTime;
        controller.Move(move);
    }

    public void FaceTo(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.0001f) return;

        RotateTo(dir);
    }

    void RotateTo(Vector3 dir)
    {
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rot,
            turnSpeed * Time.deltaTime);
    }

    public bool IsWallAhead(Vector3 dir, float checkDistance, LayerMask obstacleMask)
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        dir.y = 0;

        return Physics.Raycast(
            origin,
            dir.normalized,
            checkDistance,
            obstacleMask,
            QueryTriggerInteraction.Ignore);
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
    }
}