using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    [Header("Movement")]
    public float startSpeed = 5f;
    public float speedIncrease = 0.5f;
    public float maxBounceAngle = 60f;

    [Header("Sound")]
    public AudioClip paddleHitClip;   // drag your sound here (in Ball script inspector)
    public float minPitch = 0.85f;
    public float maxPitch = 1.35f;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 2D
    }

    void Start()
    {
        currentSpeed = startSpeed;
        Launch(Random.value < 0.5f ? Vector2.left : Vector2.right);
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * currentSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Paddle"))
            return;

        // Track last paddle hit (for powerups)
        FindObjectOfType<GameManager>()?.SetLastPaddle(collision.transform);

        // Sound
        PlayPaddleHitSound();

        // Optional camera shake (if you have CameraShake on camera)
        FindObjectOfType<CameraShake>()?.Shake();

        // Pong-style bounce
        float y = transform.position.y - collision.transform.position.y;
        float normalizedY = y / collision.collider.bounds.size.y;
        float bounceAngle = normalizedY * maxBounceAngle * Mathf.Deg2Rad;

        float dirX = rb.linearVelocity.x > 0 ? -1f : 1f; // flip X direction
        Vector2 newDir = new Vector2(Mathf.Cos(bounceAngle) * dirX, Mathf.Sin(bounceAngle));

        currentSpeed += speedIncrease;
        rb.linearVelocity = newDir.normalized * currentSpeed;
    }

    void PlayPaddleHitSound()
    {
        if (paddleHitClip == null) return;

        float speed01 = Mathf.InverseLerp(startSpeed, startSpeed + 8f, rb.linearVelocity.magnitude);
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speed01);

        audioSource.PlayOneShot(paddleHitClip);
    }

    public void ResetBall(Vector2 direction)
    {
        transform.position = Vector2.zero;
        currentSpeed = startSpeed;
        Launch(direction);
    }
}
