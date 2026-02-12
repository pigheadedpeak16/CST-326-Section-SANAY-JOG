using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Assign your 2 prefabs here")]
    public GameObject[] powerUpPrefabs;

    [Header("Spawn area around this object")]
    public float xRange = 3f;
    public float yRange = 1.5f;

    private GameObject currentPowerUp;

    public void SpawnOne()
    {
        if (currentPowerUp != null) return;

        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            Debug.LogError("PowerUpSpawner: No prefabs assigned.");
            return;
        }

        int idx = Random.Range(0, powerUpPrefabs.Length);

        float x = transform.position.x + Random.Range(-xRange, xRange);
        float y = transform.position.y + Random.Range(-yRange, yRange);

        Vector3 pos = new Vector3(x, y, 0f);

        currentPowerUp = Instantiate(powerUpPrefabs[idx], pos, Quaternion.identity);
    }

    void Update()
    {
        // when the powerup is destroyed, reference becomes null
        if (currentPowerUp == null) currentPowerUp = null;
    }
}
