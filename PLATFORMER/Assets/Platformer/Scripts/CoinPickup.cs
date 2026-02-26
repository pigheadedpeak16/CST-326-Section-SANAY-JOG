using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (UIManager.Instance != null)
            UIManager.Instance.AddCoin(1);

        Destroy(gameObject);
    }
}