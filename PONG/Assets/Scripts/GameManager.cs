using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Ball ball;                         // drag your Ball here
    public PowerUpSpawner powerUpSpawner;     // drag your PowerUpSpawner here

    [Header("UI")]
    public TextMeshProUGUI leftScoreText;     // drag LeftScoreText here
    public TextMeshProUGUI rightScoreText;    // drag RightScoreText here

    [Header("Rules")]
    public int winScore = 11;

    int leftScore = 0;
    int rightScore = 0;

    // For "grow last paddle hit" powerup
    public Transform LastPaddleHit { get; private set; }

    void Start()
    {
        UpdateScoreUI();
    }

    public void SetLastPaddle(Transform paddle)
    {
        LastPaddleHit = paddle;
    }

    // Call when RIGHT goal is hit (LEFT player scored)
    public void ScoreLeft()
    {
        leftScore++;
        UpdateScoreUI();
        StartCoroutine(ScorePop(leftScoreText));

        if (CheckWin())
        {
            ResetGame();
            return;
        }

        powerUpSpawner?.SpawnOne();
        ball?.ResetBall(Vector2.right); // send toward the player who got scored on (right side)
    }

    // Call when LEFT goal is hit (RIGHT player scored)
    public void ScoreRight()
    {
        rightScore++;
        UpdateScoreUI();
        StartCoroutine(ScorePop(rightScoreText));

        if (CheckWin())
        {
            ResetGame();
            return;
        }

        powerUpSpawner?.SpawnOne();
        ball?.ResetBall(Vector2.left); // send toward the player who got scored on (left side)
    }

    bool CheckWin()
    {
        if (leftScore >= winScore)
        {
            Debug.Log("Game Over, Left Paddle Wins");
            return true;
        }
        if (rightScore >= winScore)
        {
            Debug.Log("Game Over, Right Paddle Wins");
            return true;
        }
        return false;
    }

    void ResetGame()
    {
        leftScore = 0;
        rightScore = 0;
        UpdateScoreUI();

        // launch in a random direction after reset
        Vector2 dir = Random.value < 0.5f ? Vector2.left : Vector2.right;
        ball?.ResetBall(dir);
    }

    void UpdateScoreUI()
    {
        if (leftScoreText != null) leftScoreText.text = leftScore.ToString();
        if (rightScoreText != null) rightScoreText.text = rightScore.ToString();
    }

    IEnumerator ScorePop(TextMeshProUGUI txt)
    {
        if (txt == null) yield break;

        Vector3 originalScale = txt.transform.localScale;
        Color originalColor = txt.color;

        txt.color = Color.yellow;
        txt.transform.localScale = originalScale * 1.35f;

        yield return new WaitForSeconds(0.15f);

        txt.color = originalColor;
        txt.transform.localScale = originalScale;
    }
}
