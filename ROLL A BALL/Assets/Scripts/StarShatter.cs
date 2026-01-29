using UnityEngine;

public class StarShatter : MonoBehaviour
{
    [Header("Spawn this on collect")]
    public GameObject shatteredPrefab;

    [Header("Forces")]
    public float pushForce = 2.5f;
    public float randomTorque = 20f;
    public float upwardBoost = 0.5f;

    [Header("Cleanup")]
    public float lifetime = 4f;

    bool used;

    public void Shatter(Transform player)
    {
        if (used) return;
        used = true;

        if (shatteredPrefab != null)
        {
            GameObject shattered = Instantiate(shatteredPrefab, transform.position, transform.rotation);

            // Make sure shards can't be collected
            shattered.tag = "Untagged";
            foreach (Transform t in shattered.GetComponentsInChildren<Transform>(true))
                t.gameObject.tag = "Untagged";

            foreach (Collider c in shattered.GetComponentsInChildren<Collider>(true))
                c.isTrigger = false;

            // Push shards away from the player + random spin
            Rigidbody[] rbs = shattered.GetComponentsInChildren<Rigidbody>(true);
            foreach (Rigidbody rb in rbs)
            {
                if (rb == null) continue;

                // If shards are kinematic in prefab, make them dynamic now
                rb.isKinematic = false;

                Vector3 dir = (rb.worldCenterOfMass - player.position);
                if (dir.sqrMagnitude < 0.0001f) dir = Random.onUnitSphere;

                dir.y = Mathf.Abs(dir.y) + upwardBoost;
                rb.AddForce(dir.normalized * pushForce, ForceMode.Impulse);

                rb.AddTorque(Random.onUnitSphere * randomTorque, ForceMode.Impulse);
            }

            Destroy(shattered, lifetime);
        }

        Destroy(gameObject);
    }
}
