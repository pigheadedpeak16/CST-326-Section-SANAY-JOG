using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float accel = 30f;          // how fast we reach target speed
    public float decel = 45f;          // how fast we stop when no input

    [Header("Jump (variable height)")]
    public float jumpVelocity = 6f;     // initial jump up speed
    public float jumpHoldAcceleration = 25f; // extra upward accel while holding space
    public float jumpHoldTime = 0.18f;  // max time you can hold for extra height
    public float jumpCutMultiplier = 0.5f; // releasing early cuts jump (smaller = shorter)

    [Header("Speed Clamps (separate X/Y)")]
    public float maxFallSpeed = 25f;
    public float maxRiseSpeed = 20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundMask;

    [Header("Facing / Spawn Rotation")]
    public float spawnYaw = 90f;        // rotates once on spawn
    public float rightFacingYaw = 90f;  // when pressing D
    public bool invertFacing = false;

    [Header("Animator Params")]
    public string speedParam = "Speed";
    public string groundedParam = "IsGrounded";

    private Rigidbody rb;
    private Animator anim;

    private bool isGrounded;
    private bool jumpHolding;
    private float jumpHoldTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // Rotate once on spawn so model faces camera direction you want
        transform.rotation = Quaternion.Euler(0f, spawnYaw, 0f);
    }

    void Update()
    {
        // Ground check
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(
                groundCheck.position,
                groundCheckRadius,
                groundMask,
                QueryTriggerInteraction.Ignore
            );
        }

        // Jump pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            var v = rb.linearVelocity;
            v.y = jumpVelocity;
            rb.linearVelocity = v;

            jumpHolding = true;
            jumpHoldTimer = 0f;
        }

        // Jump held (variable height)
        if (jumpHolding && Input.GetKey(KeyCode.Space))
        {
            jumpHoldTimer += Time.deltaTime;
            if (jumpHoldTimer >= jumpHoldTime)
                jumpHolding = false;
        }

        // Jump released early => cut jump short
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpHolding = false;

            var v = rb.linearVelocity;
            if (v.y > 0f)
            {
                v.y *= jumpCutMultiplier;
                rb.linearVelocity = v;
            }
        }

        // Animator
        if (anim != null)
        {
            float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
            anim.SetFloat(speedParam, horizontalSpeed);
            anim.SetBool(groundedParam, isGrounded);
        }
    }

    void FixedUpdate()
    {
        float input = 0f;
        if (Input.GetKey(KeyCode.A)) input = -1f;
        if (Input.GetKey(KeyCode.D)) input = 1f;

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float targetSpeed = input * (sprint ? runSpeed : walkSpeed);

        // Facing rotation when input pressed
        if (input > 0.01f)
        {
            float yaw = rightFacingYaw;
            if (invertFacing) yaw += 180f;
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }
        else if (input < -0.01f)
        {
            float yaw = rightFacingYaw + 180f;
            if (invertFacing) yaw += 180f;
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        // Accelerate/decelerate horizontally
        float currentX = rb.linearVelocity.x;

        float rate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : decel;
        float newX = Mathf.MoveTowards(currentX, targetSpeed, rate * Time.fixedDeltaTime);

        // Apply jump hold extra upward acceleration
        float newY = rb.linearVelocity.y;
        if (jumpHolding && Input.GetKey(KeyCode.Space))
        {
            newY += jumpHoldAcceleration * Time.fixedDeltaTime;
        }

        // Clamp Y separately
        newY = Mathf.Clamp(newY, -maxFallSpeed, maxRiseSpeed);

        rb.linearVelocity = new Vector3(newX, newY, rb.linearVelocity.z);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}