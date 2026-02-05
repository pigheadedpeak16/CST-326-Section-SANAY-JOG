using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public float startSpeed = 5f;
    public float speedIncrease = 0.5f;
    public float maxBounceAngle = 60f;

    Rigidbody2D rb;
    Vector2 startDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Launch(Random.value < 0.5f ? Vector2.left : Vector2.right);
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * startSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 🟢 Paddle hit
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // ----- CAMERA SHAKE -----
            FindObjectOfType<CameraShake>()?.Shake();

            // ----- PONG-STYLE BOUNCE -----
            float y = transform.position.y - collision.transform.position.y;
            float normalizedY = y / collision.collider.bounds.size.y;
            float bounceAngle = normalizedY * maxBounceAngle * Mathf.Deg2Rad;

            float directionX = rb.linearVelocity.x > 0 ? -1 : 1;
            Vector2 newDir = new Vector2(Mathf.Cos(bounceAngle) * directionX,
                                         Mathf.Sin(bounceAngle));

            startSpeed += speedIncrease;
            rb.linearVelocity = newDir.normalized * startSpeed;

            // ----- OPTIONAL: EASY EXTRA CREDIT -----
            transform.localScale *= 1.03f; // size change
        }
    }

    public void ResetBall(Vector2 direction)
    {
        transform.position = Vector2.zero;
        startSpeed = 5f;
        Launch(direction);
    }
}
