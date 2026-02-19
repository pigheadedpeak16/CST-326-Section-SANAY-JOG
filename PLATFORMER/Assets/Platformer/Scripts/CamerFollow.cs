using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCameraFollow : MonoBehaviour
{
    public float moveRange = 5f;     // how far camera can shift left/right
    public float smoothSpeed = 5f;   // smoothness

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        float mouseX = Mouse.current.position.ReadValue().x;

        // Normalize mouse X (-0.5 to 0.5)
        float normalizedX = (mouseX / Screen.width) - 0.5f;

        // Calculate target X offset
        float targetX = startPosition.x + (normalizedX * moveRange);

        Vector3 targetPosition = new Vector3(
            targetX,
            startPosition.y,
            startPosition.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
