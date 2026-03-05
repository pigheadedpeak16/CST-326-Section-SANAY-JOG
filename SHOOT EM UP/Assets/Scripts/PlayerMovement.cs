using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float horizontalPadding = 0.5f;

    float leftLimit, rightLimit;

    void Start()
    {
        // compute horizontal world limits from camera
        float zDist = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        Vector3 leftWorld = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, zDist));
        Vector3 rightWorld = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, zDist));
        leftLimit = leftWorld.x + horizontalPadding;
        rightLimit = rightWorld.x - horizontalPadding;
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal"); // -1,0,1
        Vector3 pos = transform.position;
        pos.x += input * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        transform.position = pos;
    }
}