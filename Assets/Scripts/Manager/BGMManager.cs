using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource normalBGM;
    public AudioSource chaseBGM;

    private int chasingEnemyCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddChaser()
    {
        chasingEnemyCount++;

        if (chasingEnemyCount == 1)
        {
            if (normalBGM != null) normalBGM.Stop();
            if (chaseBGM != null && !chaseBGM.isPlaying) chaseBGM.Play();
        }
    }

    public void RemoveChaser()
    {
        chasingEnemyCount--;

        if (chasingEnemyCount < 0)
            chasingEnemyCount = 0;

        if (chasingEnemyCount == 0)
        {
            if (chaseBGM != null) chaseBGM.Stop();
            if (normalBGM != null && !normalBGM.isPlaying) normalBGM.Play();
        }
    }
}