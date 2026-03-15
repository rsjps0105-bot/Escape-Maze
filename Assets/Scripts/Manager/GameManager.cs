using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject loseUI;
    public GameObject resultUI;

    bool isGameOver;
    bool isResult;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isGameOver) return;

        if (!isResult)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ShowResult();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        loseUI.SetActive(true);

        //Time.timeScale = 0f;   // ÉQÅ[ÉÄí‚é~
    }

    void ShowResult()
    {
        isResult = true;

        loseUI.SetActive(false);
        resultUI.SetActive(true);
    }
}
