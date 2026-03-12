using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Scoring")]
    [Tooltip("Points awarded when this enemy is destroyed")]
    public int scoreValue = 10;

    [Header("Health")]
    public int maxHealth = 1;
    int currentHealth;

    // So EnemySpawner can assign itself
    public EnemySpawner spawner;

    [Header("Explosion FX")]
    public GameObject explosionPrefab;     // Particle System prefab
    public float explosionLifetime = 1f;   // destroy FX after this
    public float destroyDelay = 0.05f;     // delay so sound can play

    [Header("Sound (Enemy)")]
    public AudioSource audioSource;        // drag Enemy's AudioSource here
    public AudioClip explodeClip;
    [Range(0f, 1f)] public float explodeVolume = 1f;

    // Optional: if you want Enemy to be able to play shoot sound too
    // (but actual shooting happens in EnemyGroup)
    public AudioClip shootClip;
    [Range(0f, 1f)] public float shootVolume = 1f;

    void Awake()
    {
        currentHealth = maxHealth;

        // Auto-grab if you forgot to drag it
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        // Particle FX
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, explosionLifetime);
        }

        // Explosion sound (2D if AudioSource is 2D)
        if (audioSource != null && explodeClip != null)
            audioSource.PlayOneShot(explodeClip, explodeVolume);

        // score + events
        GameState.score += scoreValue;
        GameEvents.RaiseEnemyDestroyed(scoreValue);

        if (spawner != null)
            spawner.RegisterDeath();

        // Disable visuals/colliders immediately (so it "dies" now)
        DisableEnemyVisuals();

        // Destroy after a tiny delay so sound isn't cut off
        Destroy(gameObject, destroyDelay);
    }

    void DisableEnemyVisuals()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }

    // EnemyGroup can call this on the chosen shooter
    public void PlayShootSfx()
    {
        if (audioSource != null && shootClip != null)
            audioSource.PlayOneShot(shootClip, shootVolume);
    }
}