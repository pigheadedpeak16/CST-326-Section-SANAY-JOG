using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public int points = 100;

    public void HitFromBelow(UIManager ui)
    {
        if (ui != null) ui.AddScore(points);
        Destroy(gameObject);
    }
}