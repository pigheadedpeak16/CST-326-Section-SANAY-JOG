using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnPlayerDeath()
    {
        LoadCreditsSafe(1.0f);
    }

    public void OnHordeDefeated()
    {
        LoadCreditsSafe(0.5f);
    }

    void LoadCreditsSafe(float delaySeconds)
    {
        // IMPORTANT: if your game set timeScale = 0, restore it
        Time.timeScale = 1f;

        // Use realtime delay so it still works even if timeScale changes again
        StartCoroutine(LoadCreditsRealtime(delaySeconds));
    }

    IEnumerator LoadCreditsRealtime(float delaySeconds)
    {
        yield return new WaitForSecondsRealtime(delaySeconds);
        SceneManager.LoadScene("Credits");
    }
}