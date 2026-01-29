using TMPro;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public TextMeshProUGUI starText;
    public GameObject victoryText;

    int totalStars;
    int collectedStars;

    void Start()
    {
        totalStars = GameObject.FindGameObjectsWithTag("PickUp").Length;
        collectedStars = 0;

        if (victoryText != null)
            victoryText.SetActive(false);

        UpdateUI();
    }

    public void StarCollected()
    {
        collectedStars++;
        UpdateUI();

        if (collectedStars >= totalStars)
        {
            if (victoryText != null)
                victoryText.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (starText != null)
            starText.text = $"{collectedStars}/{totalStars} Stars";
    }
}
