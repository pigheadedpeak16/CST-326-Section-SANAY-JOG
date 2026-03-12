using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()   => SceneManager.LoadScene("MainMenu");
    public void LoadMainGame()   => SceneManager.LoadScene("MainGame");
    public void LoadCredits()    => SceneManager.LoadScene("Credits");
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit called (editor won't quit).");
    }
}