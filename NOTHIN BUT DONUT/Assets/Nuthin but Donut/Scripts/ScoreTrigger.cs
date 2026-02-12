using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public ScoreManager scoreManger;

    void OnTriggerExit()
    {
        scoreManger.AddScore();
    }
}