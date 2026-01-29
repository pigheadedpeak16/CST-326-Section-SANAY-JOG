using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    Rigidbody rb;

    [Header("Movement")]
    public float moveForce = 12f;
    public float maxSpeed = 8f;

    [Header("Jump")]
    public float jumpImpulse = 6f;
    public float groundCheckDistance = 0.6f;
    public LayerMask groundMask = ~0;

    bool jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpQueued = true;
    }

    void FixedUpdate()
    {
        MoveCameraRelative();
        JumpIfQueued();
        ClampHorizontalSpeed();
    }

    void MoveCameraRelative()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (cam == null)
        {
            Vector3 fallback = new Vector3(h, 0f, v).normalized;
            rb.AddForce(fallback * moveForce, ForceMode.Acceleration);
            return;
        }

        Vector3 forward = cam.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = cam.right; right.y = 0f; right.Normalize();

        Vector3 dir = (forward * v + right * h);
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        rb.AddForce(dir * moveForce, ForceMode.Acceleration);
    }

    void JumpIfQueued()
    {
        if (!jumpQueued) return;
        jumpQueued = false;

        if (!IsGrounded()) return;

        Vector3 vel = rb.linearVelocity;
        if (vel.y < 0f) vel.y = 0f;
        rb.linearVelocity = vel;

        rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore);
    }

    void ClampHorizontalSpeed()
    {
        Vector3 vel = rb.linearVelocity;
        Vector3 flat = new Vector3(vel.x, 0f, vel.z);

        if (flat.magnitude > maxSpeed)
        {
            Vector3 limited = flat.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, vel.y, limited.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PickUp")) return;

        // If star has shatter script, shatter it; otherwise destroy it
        StarShatter shatter = other.GetComponent<StarShatter>();
        if (shatter != null)
            shatter.Shatter(transform);
        else
            Destroy(other.gameObject);

        // Update UI counter
        FindFirstObjectByType<LevelUIManager>()?.StarCollected();
    }
}
