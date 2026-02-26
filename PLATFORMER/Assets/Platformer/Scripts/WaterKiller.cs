using UnityEngine;

public class HazardKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FAILED: Player touched hazard!");
            // If you have a fail UI / GameManager, call it here too.
            // GameManager.I.Fail("FAILED: You fell in water!");
        }
    }
}