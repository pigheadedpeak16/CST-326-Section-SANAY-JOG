using UnityEngine;

/// <summary>
/// Spawns a grid of enemies (multiple types) and tracks how many are alive.
/// When all enemies are dead this spawner notifies the GameManager to show Credits.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs (assign four types)")]
    public GameObject enemyTypeTop;     // highest points (top row)
    public GameObject enemyType2;
    public GameObject enemyType3;
    public GameObject enemyTypeBottom;  // lowest points (bottom row)

    [Header("Enemy Bullet Prefab (assign)")]
    public GameObject enemyBulletPrefab;  // <--- DRAG EnemyBullet prefab here

    [Header("Layout")]
    public int columns = 8;
    public int rows = 4;
    public Vector2 spacing = new Vector2(1.2f, 1.0f);
    public Vector2 startPosition = new Vector2(-6f, 3.5f);

    [Header("Optional")]
    [Tooltip("If set, this transform will be used as the parent for spawned enemies. If null a new GameObject 'EnemyGroup' will be created.")]
    public Transform groupParent;

    // Public read-only count (inspector visible for debugging)
    [SerializeField] private int enemiesRemaining = 0;
    public int EnemiesRemaining => enemiesRemaining;

    void Start()
    {
        // Basic validation
        if (enemyTypeTop == null || enemyType2 == null || enemyType3 == null || enemyTypeBottom == null)
        {
            Debug.LogError("EnemySpawner: Assign all 4 enemy type prefabs in the inspector.");
            return;
        }

        if (enemyBulletPrefab == null)
        {
            Debug.LogError("EnemySpawner: Assign enemyBulletPrefab (EnemyBullet prefab) in the inspector.");
            return;
        }

        // Create or use a group parent
        GameObject groupGO;
        if (groupParent != null)
        {
            groupGO = groupParent.gameObject;
            groupGO.name = groupGO.name == "" ? "EnemyGroup" : groupGO.name;
        }
        else
        {
            groupGO = new GameObject("EnemyGroup");
            groupParent = groupGO.transform;
        }

        // Ensure an EnemyGroup component exists and assign the bullet prefab
        EnemyGroup eg = groupGO.GetComponent<EnemyGroup>();
        if (eg == null) eg = groupGO.AddComponent<EnemyGroup>();
        eg.enemyBulletPrefab = enemyBulletPrefab;

        // Spawn grid and register enemies
        enemiesRemaining = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = new Vector3(
                    startPosition.x + c * spacing.x,
                    startPosition.y - r * spacing.y,
                    0f
                );

                GameObject prefabToUse = PrefabForRow(r);
                GameObject inst = Instantiate(prefabToUse, pos, Quaternion.identity, groupParent);

                // If the instantiated prefab has an Enemy component, assign this spawner and increment counter.
                Enemy enemyComp = inst.GetComponent<Enemy>();
                if (enemyComp != null)
                {
                    enemyComp.spawner = this; // requires Enemy.spawner to be public/internal
                    RegisterSpawn();
                }
                else
                {
                    // If prefab doesn't have Enemy script, still treat it as an enemy for counting
                    RegisterSpawn();
                }
            }
        }

        // Optional sanity log
        Debug.Log($"EnemySpawner: Spawned {enemiesRemaining} enemies (rows {rows} x cols {columns}).");
    }

    GameObject PrefabForRow(int r)
    {
        int index = r % 4;
        switch (index)
        {
            case 0: return enemyTypeTop;
            case 1: return enemyType2;
            case 2: return enemyType3;
            default: return enemyTypeBottom;
        }
    }

    /// <summary>
    /// Call when an enemy has been spawned (we use it while instantiating).
    /// If your Enemy script already calls RegisterSpawn() itself, you can remove this usage.
    /// </summary>
    public void RegisterSpawn()
    {
        enemiesRemaining++;
    }

    /// <summary>
    /// Call when an enemy dies. When count reaches zero we notify the GameManager.
    /// </summary>
    public void RegisterDeath()
    {
        enemiesRemaining--;
        if (enemiesRemaining < 0) enemiesRemaining = 0;

        // Debug log for visibility while testing
        Debug.Log($"EnemySpawner: Enemy died. Remaining={enemiesRemaining}");

        if (enemiesRemaining <= 0)
        {
            // All enemies are dead — notify GameManager
            if (GameManager.I != null)
            {
                GameManager.I.OnHordeDefeated();
            }
            else
            {
                Debug.LogWarning("EnemySpawner: GameManager instance not found - loading Credits scene directly.");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
            }
        }
    }
}