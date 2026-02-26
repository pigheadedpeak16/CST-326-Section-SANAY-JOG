using UnityEngine;

public class CameraEdgePan : MonoBehaviour
{
    [Header("Pan Settings")]
    public float panSpeed = 6f;          // units per second
    public float edgePercent = 0.06f;    // 6% of screen width on each side

    [Header("Locks")]
    public bool lockY = true;
    public bool lockZ = true;

    private float lockedY;
    private float lockedZ;

    void Start()
    {
        lockedY = transform.position.y;
        lockedZ = transform.position.z;
    }

    void Update()
    {
        float x = transform.position.x;

        float edgePixels = Screen.width * edgePercent;
        float mouseX = Input.mousePosition.x;

        float dir = 0f;

        if (mouseX <= edgePixels) dir = -1f;                         // left edge
        else if (mouseX >= Screen.width - edgePixels) dir = 1f;      // right edge

        x += dir * panSpeed * Time.deltaTime;

        float y = lockY ? lockedY : transform.position.y;
        float z = lockZ ? lockedZ : transform.position.z;

        transform.position = new Vector3(x, y, z);
    }
}