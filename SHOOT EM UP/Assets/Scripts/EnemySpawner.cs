using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs (assign four types)")]
    public GameObject enemyTypeTop;     // highest points (top row)
    public GameObject enemyType2;
    public GameObject enemyType3;
    public GameObject enemyTypeBottom;  // lowest points (bottom row)

    [Header("Enemy Bullet Prefab")]
    public GameObject enemyBulletPrefab;  // <--- DRAG EnemyBullet prefab here

    [Header("Layout")]
    public int columns = 8;
    public int rows = 4;
    public Vector2 spacing = new Vector2(1.2f, 1.0f);
    public Vector2 startPosition = new Vector2(-6f, 3.5f);

    void Start()
    {
        if (enemyTypeTop == null || enemyType2 == null || enemyType3 == null || enemyTypeBottom == null)
        {
            Debug.LogError("EnemySpawner: Assign all 4 enemy type prefabs.");
            return;
        }

        if (enemyBulletPrefab == null)
        {
            Debug.LogError("EnemySpawner: Assign enemyBulletPrefab (EnemyBullet prefab).");
            return;
        }

        GameObject group = new GameObject("EnemyGroup");
        EnemyGroup eg = group.AddComponent<EnemyGroup>();
        eg.enemyBulletPrefab = enemyBulletPrefab; // <--- THIS FIXES SHOOTING

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
                Instantiate(prefabToUse, pos, Quaternion.identity, group.transform);
            }
        }
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
}