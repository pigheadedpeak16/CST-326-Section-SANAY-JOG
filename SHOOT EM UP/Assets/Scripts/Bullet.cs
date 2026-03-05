using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 4f;
    public int damage = 1;

    float t;

    void OnEnable() => t = lifeTime;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);

        t -= Time.deltaTime;
        if (t <= 0f) Destroy(gameObject);

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.y > 1.2f || vp.y < -0.2f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null) e.TakeDamage(damage);
            else Destroy(other.gameObject);

            Destroy(gameObject);
            return;
        }

        // no barricades in your current build
    }
}