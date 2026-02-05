using UnityEngine;

public class BallDropper : MonoBehaviour
{
    [Header("Prefab to spawn")]
    public GameObject ballPrefab;

    [Header("Spawn area")]
    public Transform spawnCenter;      // optional; can be this object
    public float randomXRange = 3f;    // +/- range from center
    public float spawnHeight = 8f;     // how high above center

    [Header("Optional")]
    public float cooldown = 0.2f;      // prevent spam
    float lastSpawnTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastSpawnTime >= cooldown)
        {
            SpawnBall();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnBall()
    {
        Transform c = spawnCenter != null ? spawnCenter : transform;

        float x = c.position.x + Random.Range(-randomXRange, randomXRange);
        Vector3 spawnPos = new Vector3(x, c.position.y + spawnHeight, c.position.z);

        Instantiate(ballPrefab, spawnPos, Quaternion.identity);
    }
}
