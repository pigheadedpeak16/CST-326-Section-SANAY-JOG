using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public InputActionReference moveAction;

    float minY;
    float maxY;

    void Awake()
    {
        // Paddle half height
        float paddleHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        // Camera bounds
        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camCenterY = cam.transform.position.y;

        // Clamp limits
        minY = camCenterY - camHalfHeight + paddleHalfHeight;
        maxY = camCenterY + camHalfHeight - paddleHalfHeight;
    }

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        float inputY = moveAction.action.ReadValue<Vector2>().y;

        float newY = transform.position.y + inputY * speed * Time.deltaTime;
        newY = Mathf.Clamp(newY, minY, maxY);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
