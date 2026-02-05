using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool isLeftGoal; // check this on LeftGoal only
    GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        if (isLeftGoal) gm.ScoreRight();
        else gm.ScoreLeft();
    }
}
