using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;
    bool isDead = false;

    [Header("Explosion FX")]
    public GameObject explosionPrefab;
    public float explosionLifetime = 1f;
    public float deathDelay = 0.35f; // small delay so explosion is visible

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip explodeClip;

    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("GAME OVER");

        // explosion particle
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, explosionLifetime);
        }

        // sound
        if (audioSource != null && explodeClip != null)
            audioSource.PlayOneShot(explodeClip);

        // stop player control/visibility but keep game running
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // keep your existing event system (optional)
        GameEvents.RaisePlayerDied();

        // do NOT pause the game
        Time.timeScale = 1f;

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);

        // switch to Credits (rubric requirement)
        if (GameManager.I != null)
            GameManager.I.OnPlayerDeath();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");

        gameObject.SetActive(false);
    }
}