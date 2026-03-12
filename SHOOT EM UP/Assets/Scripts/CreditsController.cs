using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CreditsController : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        nameText.text = GameState.playerName;
        scoreText.text = "Points: " + GameState.score;

        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
        GameState.ResetForNewRun();
    }
}