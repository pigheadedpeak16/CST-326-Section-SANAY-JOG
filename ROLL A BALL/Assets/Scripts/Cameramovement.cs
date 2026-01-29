using UnityEngine;

public class OrbitChaseCamera : MonoBehaviour
{
    public Transform target;

    [Header("Distance / Height")]
    public float distance = 8f;
    public float height = 3f;

    [Header("Mouse")]
    public float mouseSensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 70f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.08f;
    public float rotationSmoothSpeed = 12f;

    [Header("Zoom (optional)")]
    public bool enableZoom = true;
    public float zoomSpeed = 2f;
    public float minDistance = 3f;
    public float maxDistance = 15f;

    private float yaw;
    private float pitch;
    private Vector3 positionVelocity;

    void Start()
    {
        if (!target) return;

        // Start from current camera rotation
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        // Lock cursor for game feel (press Esc to unlock in editor)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!target) return;

        // Mouse look
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Optional zoom with scroll wheel
        if (enableZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.0001f)
            {
                distance -= scroll * zoomSpeed * 5f;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // Build the desired rotation from yaw/pitch
        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Desired camera position (behind the target relative to rotation)
        Vector3 desiredPosition =
            target.position
            + desiredRotation * new Vector3(0f, height, -distance);

        // Smooth position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref positionVelocity,
            positionSmoothTime
        );

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
