using UnityEngine;
using UnityEngine.InputSystem;

public class LevelParser : MonoBehaviour
{
    public TextAsset levelFile;
    public Transform levelRoot;

    [Header("Prefabs")]
    public GameObject rockPrefab;         // x
    public GameObject brickPrefab;        // b
    public GameObject questionBoxPrefab;  // ?
    public GameObject strongPrefab;       // s
    public GameObject waterPrefab;        // w
    public GameObject goalPrefab;         // g
    public GameObject playerPrefab;       // p

    [Header("Grid Settings")]
    public float cellSize = 1f;
    public Vector3 origin = Vector3.zero;

    void Start()
    {
        LoadLevel();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            ReloadLevel();
    }

    void LoadLevel()
    {
        if (levelFile == null)
        {
            Debug.LogError("LevelParser: levelFile is not assigned!");
            return;
        }

        if (levelRoot == null)
        {
            Debug.LogError("LevelParser: levelRoot is not assigned!");
            return;
        }

        // Handles Windows line endings safely.
        string[] lines = levelFile.text.Replace("\r", "").Split('\n');

        // Bottom of the level should be the LAST line in the file.
        int totalRows = lines.Length;

        for (int fileRow = 0; fileRow < totalRows; fileRow++)
        {
            string rowString = lines[fileRow];

            // Map file row to world row so bottom is y=0
            int worldRow = (totalRows - 1) - fileRow;

            // If the row is empty, it's just an empty row. Keep it.
            if (string.IsNullOrEmpty(rowString))
                continue;

            for (int col = 0; col < rowString.Length; col++)
            {
                char c = rowString[col];
                GameObject prefabToSpawn = null;

                // Map letters to prefabs
                if (c == 'x') prefabToSpawn = rockPrefab;
                else if (c == 'b') prefabToSpawn = brickPrefab;
                else if (c == '?') prefabToSpawn = questionBoxPrefab;
                else if (c == 's') prefabToSpawn = strongPrefab;
                else if (c == 'w') prefabToSpawn = waterPrefab;
                else if (c == 'g') prefabToSpawn = goalPrefab;
                else if (c == 'p') prefabToSpawn = playerPrefab;
                else continue; // ignore spaces/unknown chars

                // If a prefab wasn't assigned in Inspector, skip gracefully
                if (prefabToSpawn == null)
                {
                    Debug.LogWarning($"LevelParser: No prefab assigned for '{c}' (column {col}, row {worldRow}).");
                    continue;
                }

                Vector3 pos = origin + new Vector3(col * cellSize, worldRow * cellSize, 0f);
                Instantiate(prefabToSpawn, pos, Quaternion.identity, levelRoot);
            }
        }
    }

    void ReloadLevel()
    {
        if (levelRoot == null) return;

        for (int i = levelRoot.childCount - 1; i >= 0; i--)
            Destroy(levelRoot.GetChild(i).gameObject);

        LoadLevel();
    }
}