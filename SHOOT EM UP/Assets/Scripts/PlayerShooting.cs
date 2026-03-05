using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;      // child transform above player
    public GameObject bulletPrefab;  // player bullet prefab (set in inspector)

    [Header("Fire settings")]
    public float fireRate = 5f;      // bullets per second
    public bool holdToFire = true;   // true => hold space to auto fire, false => one-shot per press

    float cooldown;

    void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;

        if (holdToFire)
        {
            if (Input.GetKey(KeyCode.Space) && cooldown <= 0f)
            {
                Shoot();
                cooldown = 1f / fireRate;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && cooldown <= 0f)
            {
                Shoot();
                cooldown = 1f / fireRate;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // (optional) add audio here
    }
}