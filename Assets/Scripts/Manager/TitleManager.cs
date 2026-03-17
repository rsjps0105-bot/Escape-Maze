using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Update()
    {
        // âΩÇ©ÉLÅ[âüÇ≥ÇÍÇΩÇÁ
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}