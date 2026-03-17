using UnityEngine;
using System.Collections;

public class PitFallTrigger : MonoBehaviour
{
    [Header("Target")]
    public string playerTag = "Player";

    [Header("Animation")]
    public string fallTriggerName = "Fall";

    [Header("Fall")]
    public float fallSpeed = 4f;
    public float gameOverDelay = 1.5f;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        StartCoroutine(FallSequence(other.gameObject));
    }

    private IEnumerator FallSequence(GameObject player)
    {
        Animator animator = player.GetComponentInChildren<Animator>();
        PlayerMove controller = player.GetComponent<PlayerMove>();

        // “ü—Í‚¾‚¯~‚ß‚é
        if (controller != null)
            controller.enabled = false;

        // —‰ºƒAƒjƒ
        if (animator != null)
            animator.SetTrigger(fallTriggerName);

        float timer = 0f;

        // ˆê’èŠÔA—‚¿‘±‚¯‚é
        while (timer < gameOverDelay)
        {
            timer += Time.deltaTime;
            player.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }
}