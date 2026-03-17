using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    public string winTriggerName = "Win";
    public float clearDelay = 2f;

    bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        StartCoroutine(ClearSequence(other.gameObject));
    }

    IEnumerator ClearSequence(GameObject player)
    {
        Animator animator = player.GetComponentInChildren<Animator>();
        PlayerMove controller = player.GetComponent<PlayerMove>();

        // ëÄçÏí‚é~
        if (controller != null)
            controller.enabled = false;

        // WinÉAÉjÉÅçƒê∂
        if (animator != null)
            animator.SetTrigger(winTriggerName);

        yield return new WaitForSeconds(clearDelay);

        GameManager.Instance.GameClear();
    }
}