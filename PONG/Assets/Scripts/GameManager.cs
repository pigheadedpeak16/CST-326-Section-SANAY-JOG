using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public int leftScore = 0;
    public int rightScore = 0;
    public int scoreToWin = 11;

    [Header("References")]
    public Ball ball;

    [Header("UI (TextMeshPro)")]
    public TMP_Text leftScoreText;
    public TMP_Text rightScoreText;
    public TMP_Text messageText; // optional (can be null)

    void Start()
    {
        UpdateUI();
        SetMessage(""); // clear message on start
    }

    // Call when LEFT player scores (ball went into RIGHT goal)
    public void ScoreLeft()
    {
        leftScore++;
        Debug.Log($"Left scores! {leftScore} - {rightScore}");
        UpdateUI();

        if (CheckGameOver()) return;

        // Send ball toward the player who got scored on (RIGHT got scored on)
        ball.ResetBall(Vector2.right);
    }

    // Call when RIGHT player scores (ball went into LEFT goal)
    public void ScoreRight()
    {
        rightScore++;
        Debug.Log($"Right scores! {leftScore} - {rightScore}");
        UpdateUI();

        if (CheckGameOver()) return;

        // Send ball toward the player who got scored on (LEFT got scored on)
        ball.ResetBall(Vector2.left);
    }

    bool CheckGameOver()
    {
        if (leftScore >= scoreToWin || rightScore >= scoreToWin)
        {
            string winner = leftScore >= scoreToWin ? "Left" : "Right";
            Debug.Log($"Game Over, {winner} Paddle Wins");

            SetMessage($"Game Over!\n{winner} Wins");

            // Reset scores
            leftScore = 0;
            rightScore = 0;
            UpdateUI();

            // Restart ball in a neutral/random direction after game over
            Vector2 dir = Random.value < 0.5f ? Vector2.left : Vector2.right;
            ball.ResetBall(dir);

            return true;
        }

        return false;
    }

    void UpdateUI()
    {
        if (leftScoreText != null) leftScoreText.text = leftScore.ToString();
        if (rightScoreText != null) rightScoreText.text = rightScore.ToString();
    }

    void SetMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }
}
