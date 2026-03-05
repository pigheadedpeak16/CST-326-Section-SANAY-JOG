using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 6f;

    float timer;

    void OnEnable() => timer = lifeTime;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
            Destroy(gameObject);

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.y < -0.2f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}