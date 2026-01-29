using UnityEngine;

public class StarCollectible : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationSpeed = new Vector3(0f, 120f, 0f);

    [Header("Floating")]
    public float floatAmplitude = 0.25f;   // how high it moves
    public float floatFrequency = 2f;      // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);

        // Float up & down using sine wave
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + Vector3.up * yOffset;
    }
}
