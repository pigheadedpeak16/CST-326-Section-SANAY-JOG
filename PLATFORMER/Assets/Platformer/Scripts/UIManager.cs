using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timeText;

    public int score = 0;
    public int coins = 0;
    public int startTime = 300;

    private float currentTime;

    void Start()
    {
        currentTime = startTime;
        UpdateUI();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        coinText.text = "Coins: " + coins;
        timeText.text = "Time: " + currentTime;
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        score += 100 * amount;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }
}
