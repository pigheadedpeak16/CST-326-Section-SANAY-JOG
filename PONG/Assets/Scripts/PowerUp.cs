using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerType { GrowLastPaddle, SpeedBall }
    public PowerType type;

    public float duration = 4f;

    [Header("Grow Last Paddle")]
    public float growMultiplier = 1.5f;

    [Header("Speed Ball")]
    public float speedMultiplier = 1.4f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        if (type == PowerType.GrowLastPaddle)
        {
            StartCoroutine(GrowLastPaddleRoutine());
        }
        else if (type == PowerType.SpeedBall)
        {
            StartCoroutine(SpeedBallRoutine(other.attachedRigidbody));
        }

        Destroy(gameObject); // pickup disappears immediately
    }

    IEnumerator GrowLastPaddleRoutine()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null || gm.LastPaddleHit == null) yield break;

        Transform paddle = gm.LastPaddleHit;

        // If paddle already has an active size powerup, stop stacking (reset first)
        PaddlePowerState state = paddle.GetComponent<PaddlePowerState>();
        if (state == null) state = paddle.gameObject.AddComponent<PaddlePowerState>();

        // reset to original first (prevents permanent growth / stacking)
        state.ResetToOriginal();

        // apply grow
        state.ApplyGrow(growMultiplier);

        yield return new WaitForSeconds(duration);

        if (paddle != null)
            state.ResetToOriginal();
    }

    IEnumerator SpeedBallRoutine(Rigidbody2D ballRb)
    {
        if (ballRb == null) yield break;

        float originalSpeed = ballRb.linearVelocity.magnitude;
        ballRb.linearVelocity *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        if (ballRb != null && ballRb.linearVelocity.sqrMagnitude > 0.001f)
            ballRb.linearVelocity = ballRb.linearVelocity.normalized * originalSpeed;
    }
}

// Helper component stored on paddles to avoid stacking bugs
public class PaddlePowerState : MonoBehaviour
{
    private Vector3 originalScale;
    private bool hasOriginal;

    void Awake()
    {
        if (!hasOriginal)
        {
            originalScale = transform.localScale;
            hasOriginal = true;
        }
    }

    public void ApplyGrow(float multiplier)
    {
        if (!hasOriginal)
        {
            originalScale = transform.localScale;
            hasOriginal = true;
        }

        transform.localScale = new Vector3(originalScale.x, originalScale.y * multiplier, originalScale.z);
    }

    public void ResetToOriginal()
    {
        if (!hasOriginal)
        {
            originalScale = transform.localScale;
            hasOriginal = true;
        }

        transform.localScale = originalScale;
    }
}
