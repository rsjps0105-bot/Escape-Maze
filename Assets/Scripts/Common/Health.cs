using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("HP")]
    public float maxHP = 100f;

    public float CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0f;

    [Header("Events")]
    public UnityEvent onDamaged;
    public UnityEvent onDied;

    Animator anim;
    PlayerMove playerMove;

    void Awake()
    {
        CurrentHP = maxHP;
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0f);

        if (IsDead)
        {
            Die();
            return;
        }

        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        onDamaged?.Invoke();
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, maxHP);
    }

    void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("Lose");
        }

        if (playerMove != null)
        {
            playerMove.enabled = false;
        }

        onDied?.Invoke();

        Debug.Log($"{name} died");
    }
}
