using UnityEngine;

public class OrbitAroundSun : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform center;              // Sun
    public float orbitSpeed = 10f;         // Degrees per second

    void Update()
    {
        if (center == null) return;

        transform.RotateAround(
            center.position,
            Vector3.up,
            orbitSpeed * Time.deltaTime
        );
    }
}
