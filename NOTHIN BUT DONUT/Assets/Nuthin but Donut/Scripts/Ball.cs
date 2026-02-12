using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public AudioClip trampolineCollision;
    public AudioClip rimCollision;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"I hit the {collision.collider.name}");
        if (collision.gameObject.CompareTag("Trampoline"))
            audioSource.PlayOneShot(trampolineCollision);
        
        // Todo - add a tag for the rim and play a sound in response
    }
}