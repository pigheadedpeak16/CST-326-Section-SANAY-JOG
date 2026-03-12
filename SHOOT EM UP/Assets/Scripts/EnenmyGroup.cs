using System.Collections;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [Header("Step Movement")]
    public float stepAmount = 0.4f;
    public float stepDownAmount = 0.6f;
    public float baseInterval = 0.7f;
    public float minInterval = 0.12f;

    [Header("Firing")]
    public GameObject enemyBulletPrefab;
    public float fireInterval = 2f;

    [Header("Shoot Animation")]
    public string shootTriggerName = "Shoot";

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip enemyShootClip;

    Vector3 direction = Vector3.right;

    int initialCount;
    Coroutine moveRoutine;
    bool gameEnded = false;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        initialCount = transform.childCount;

        moveRoutine = StartCoroutine(MoveLoop());
        InvokeRepeating(nameof(FireBullet), 1f, fireInterval);

        GameEvents.EnemyDestroyed += OnEnemyDestroyed;
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentInterval());

            transform.Translate(direction * stepAmount);

            if (ReachedEdge())
            {
                transform.Translate(Vector3.down * stepDownAmount);
                direction = -direction;
            }
        }
    }

    float CurrentInterval()
    {
        if (initialCount == 0) return baseInterval;

        float aliveRatio = (float)transform.childCount / initialCount;
        return Mathf.Lerp(minInterval, baseInterval, aliveRatio);
    }

    bool ReachedEdge()
    {
        if (transform.childCount == 0) return false;

        float left = float.MaxValue;
        float right = float.MinValue;

        foreach (Transform child in transform)
        {
            if (child == null) continue;

            float x = child.GetComponent<SpriteRenderer>().bounds.center.x;
            left = Mathf.Min(left, x);
            right = Mathf.Max(right, x);
        }

        Vector3 leftVP = Camera.main.WorldToViewportPoint(new Vector3(left, 0, 0));
        Vector3 rightVP = Camera.main.WorldToViewportPoint(new Vector3(right, 0, 0));

        return leftVP.x <= 0f || rightVP.x >= 1f;
    }

    void FireBullet()
    {
        if (enemyBulletPrefab == null) return;
        if (transform.childCount == 0) return;

        Transform shooter = transform.GetChild(Random.Range(0, transform.childCount));

        // Trigger shoot animation
        shooter.GetComponent<Animator>()?.SetTrigger(shootTriggerName);

        // Play enemy shooting sound
        if (audioSource != null && enemyShootClip != null)
            audioSource.PlayOneShot(enemyShootClip);

        Instantiate(enemyBulletPrefab, shooter.position, Quaternion.identity);
    }

    void OnEnemyDestroyed(int points)
    {
        if (transform.childCount <= 1 && !gameEnded)
        {
            gameEnded = true;
            EndGame();
            return;
        }

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(MoveLoop());
        }
    }

    void EndGame()
    {
        Debug.Log("YOU WIN");

        CancelInvoke(nameof(FireBullet));
        StopAllCoroutines();

        Time.timeScale = 0f;
    }

    void OnDestroy()
    {
        GameEvents.EnemyDestroyed -= OnEnemyDestroyed;
    }
}