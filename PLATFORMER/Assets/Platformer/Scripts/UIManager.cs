using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI messageText; // optional

    [Header("Values")]
    public int score = 0;
    public int coins = 0;

    [Header("Timer")]
    public float startTime = 100f;

    private float currentTime;
    private bool failed = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        currentTime = startTime;
        failed = false;

        if (messageText != null)
            messageText.text = "";

        UpdateUI();
    }

    void Update()
    {
        if (failed) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            failed = true;

            Debug.Log("PLAYER FAILED: Did not reach the goal in time.");

            if (messageText != null)
                messageText.text = "FAILED: Time Up!";
        }

        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        score += 100 * amount; // 100 points per coin
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (coinText != null) coinText.text = "Coins: " + coins;
        if (timeText != null) timeText.text = "Time: " + Mathf.CeilToInt(currentTime);
    }
}