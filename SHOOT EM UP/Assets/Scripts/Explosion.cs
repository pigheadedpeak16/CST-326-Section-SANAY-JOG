using UnityEngine;

public class DestroyAfterParticles : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}