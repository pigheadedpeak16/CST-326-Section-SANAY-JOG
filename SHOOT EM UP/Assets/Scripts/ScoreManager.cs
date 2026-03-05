// ScoreManager.cs — TextMeshPro version
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI references (TextMeshPro)")]
    public TMP_Text scoreLabel;      // the "SCORE" label (optional)
    public TMP_Text scoreValueText;  // numeric text under SCORE
    public TMP_Text hiScoreLabel;    // the "HI-SCORE" label (optional)
    public TMP_Text hiScoreValueText; // numeric text under HI-SCORE

    [Header("Settings")]
    public int pointsPerEnemyDefault = 10;
    public int digits = 4;       // how many digits to show with leading zeros

    int score = 0;
    int hiScore = 0;
    string highScoreKey = "HI_SCORE_SESSION";

    void Awake()
    {
        // load session high score
        hiScore = PlayerPrefs.GetInt(highScoreKey, 0);
        UpdateUI();
    }

    void OnEnable()
    {
        GameEvents.EnemyDestroyed += OnEnemyDestroyed;
        GameEvents.PlayerDied += OnPlayerDied; // optional
    }

    void OnDisable()
    {
        GameEvents.EnemyDestroyed -= OnEnemyDestroyed;
        GameEvents.PlayerDied -= OnPlayerDied;
    }

    void OnEnemyDestroyed(int points)
    {
        score += points;
        if (score > hiScore)
        {
            hiScore = score;
            PlayerPrefs.SetInt(highScoreKey, hiScore);
            PlayerPrefs.Save();
        }
        UpdateUI();
    }

    void OnPlayerDied()
    {
        // Optionally show something / lock UI when player dies — keep score as-is
        UpdateUI();
    }

    void UpdateUI()
    {
        // Format with leading zeros: "D4" => 0000 ... 9999
        string format = "D" + digits;
        if (scoreValueText != null) scoreValueText.text = score.ToString(format);
        if (hiScoreValueText != null) hiScoreValueText.text = hiScore.ToString(format);
    }

    // Optional public methods:
    public void AddPoints(int pts) => OnEnemyDestroyed(pts);
    public int GetScore() => score;
    public int GetHiScore() => hiScore;
}