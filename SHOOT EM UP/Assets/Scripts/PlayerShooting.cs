using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Fire settings")]
    public float fireRate = 5f;
    public bool holdToFire = true;

    [Header("Animation")]
    public Animator animator;
    public string shootTriggerName = "Shoot";

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip shootClip;

    float cooldown;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;

        bool wantsFire = holdToFire
            ? Input.GetKey(KeyCode.Space)
            : Input.GetKeyDown(KeyCode.Space);

        if (wantsFire && cooldown <= 0f)
        {
            Shoot();
            cooldown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // spawn bullet
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // animation
        if (animator != null)
            animator.SetTrigger(shootTriggerName);

        // sound
        if (audioSource != null && shootClip != null)
            audioSource.PlayOneShot(shootClip);
    }
}