using UnityEngine;

public class BlockClickHandler : MonoBehaviour
{
    [Header("Raycast")]
    public Camera mainCamera;
    public LayerMask clickableLayers = ~0; // everything by default
    public float maxDistance = 200f;

    [Header("Optional: Assign your UIManager if you need coin/score updates")]
    public UIManager uiManager;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, clickableLayers))
            {
                GameObject clicked = hit.collider.gameObject;

                // If you clicked a child collider, you might want the root prefab object:
                // clicked = hit.collider.transform.root.gameObject;

                // Decide what was clicked by name OR tag (tag is better if you set tags)
                string n = clicked.name.ToLower();

                // BRICK: destroy
                if (n.Contains("brick"))
                {
                    Destroy(clicked);
                    if (uiManager != null) uiManager.score += 1;  // optional scoring
                }
                // QUESTION BOX: give coin
                else if (n.Contains("question"))
                {
                    if (uiManager != null) uiManager.coins += 1;
                }

                if (uiManager != null)
                    uiManager.UpdateUI();
            }
        }
    }
}
