using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    [Header("Rewards")]
    public int coinAmount = 1;     // 1 coin
    public int bonusScore = 0;     // optional extra (UIManager already gives 100 per coin)

    private bool used = false;

    public void HitFromBelow(UIManager ui)
    {
        if (used) return;
        used = true;

        if (ui != null)
        {
            ui.AddCoin(coinAmount);     // adds coin + 100 points each (your UIManager does this)
            if (bonusScore != 0) ui.AddScore(bonusScore);
        }

        Destroy(gameObject);
    }
}