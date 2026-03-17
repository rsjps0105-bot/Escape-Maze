using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject resultUI;
    public TMP_Text resultTitleText;
    public TMP_Text resultMessageText;

    [Header("Delay")]
    public float resultDelay = 2f;

    [Header("SE")]
    public AudioSource seSource;
    public AudioClip clearSE;
    public AudioClip gameOverSE;

    bool isFinished;
    float elapsedTime;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isFinished)
        {
            elapsedTime += Time.deltaTime;
            return;
        }

        if (resultUI != null && resultUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void GameOver()
    {
        if (isFinished) return;
        isFinished = true;

        StopAllBGM();

        if (seSource != null && gameOverSE != null)
        {
            seSource.PlayOneShot(gameOverSE);
        }

        StartCoroutine(ShowResultDelay(false));
    }

    public void GameClear()
    {
        if (isFinished) return;
        isFinished = true;

        StopAllBGM();

        if (seSource != null && clearSE != null)
        {
            seSource.PlayOneShot(clearSE);
        }

        StartCoroutine(ShowResultDelay(true));
    }

    IEnumerator ShowResultDelay(bool isClear)
    {
        yield return new WaitForSeconds(resultDelay);

        if (resultUI != null) resultUI.SetActive(true);
        if (resultTitleText != null) resultTitleText.text = "RESULT";

        if (resultMessageText != null)
        {
            if (isClear)
                resultMessageText.text = "CLEAR TIME : " + FormatTime(elapsedTime);
            else
                resultMessageText.text = "ÉNÉäÉAÇ≈Ç´Ç»Ç©Ç¡ÇΩÅc";
        }
    }

    void StopAllBGM()
    {
        if (BGMManager.Instance == null) return;

        if (BGMManager.Instance.normalBGM != null)
            BGMManager.Instance.normalBGM.Stop();

        if (BGMManager.Instance.chaseBGM != null)
            BGMManager.Instance.chaseBGM.Stop();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}