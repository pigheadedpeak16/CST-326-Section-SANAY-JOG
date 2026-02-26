using UnityEngine;

public class HeadBlockHit : MonoBehaviour
{
    public LayerMask blockMask;
    public float mustBeAboveBy = 0.05f; // block must be above head by this much

    private UIManager ui;

    void Awake()
    {
        ui = FindFirstObjectByType<UIManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Layer check
        if (((1 << other.gameObject.layer) & blockMask) == 0) return;

        // Make sure we are actually hitting the block from below (block above head trigger)
        if (other.bounds.min.y < transform.position.y + mustBeAboveBy) return;

        // Breakable brick?
        BreakableBlock brick = other.GetComponentInParent<BreakableBlock>();
        if (brick != null)
        {
            brick.HitFromBelow(ui);
            return;
        }

        // Question block?
        QuestionBlock qb = other.GetComponentInParent<QuestionBlock>();
        if (qb != null)
        {
            qb.HitFromBelow(ui);
            return;
        }
    }
}