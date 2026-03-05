using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Points awarded when this enemy is destroyed")]
    public int scoreValue = 10;

    public int maxHealth = 1;
    int currentHealth;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        // notify score manager / other listeners with this enemy's value
        GameEvents.RaiseEnemyDestroyed(scoreValue);
        Destroy(gameObject);
    }
}