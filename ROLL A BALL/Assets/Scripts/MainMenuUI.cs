using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Called by the PLAY button
    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    // Optional: quit button later
    public void Quit()
    {
        Application.Quit();
    }
}
