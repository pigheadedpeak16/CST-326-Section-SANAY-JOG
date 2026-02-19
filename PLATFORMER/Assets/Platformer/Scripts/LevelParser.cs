using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelParser : MonoBehaviour
{
    public TextAsset levelFile;
    public Transform levelRoot;

    [Header("Prefabs")]
    public GameObject rockPrefab;
    public GameObject brickPrefab;
    public GameObject questionBoxPrefab;
    public GameObject strongPrefab;

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

        // Push lines onto a stack so we can pop bottom-up rows.
        Stack<string> levelRows = new Stack<string>();

        foreach (string line in levelFile.text.Split('\n'))
            levelRows.Push(line);

        int row = 0;

        while (levelRows.Count > 0)
        {
            string rowString = levelRows.Pop();

            // Remove carriage returns in case file uses Windows line endings (\r\n)
            rowString = rowString.Replace("\r", "");

            char[] rowChars = rowString.ToCharArray();

            for (int columnIndex = 0; columnIndex < rowChars.Length; columnIndex++)
            {
                char currentChar = rowChars[columnIndex];

                GameObject prefabToSpawn = null;

                // Map characters in the text file to prefabs
                if (currentChar == 'x') prefabToSpawn = rockPrefab;
                else if (currentChar == 'b') prefabToSpawn = brickPrefab;
                else if (currentChar == '?') prefabToSpawn = questionBoxPrefab;
                else if (currentChar == 's') prefabToSpawn = strongPrefab;
                else
                    continue; // ignore spaces/unknown chars

                // Position on a simple grid (1 unit per char)
                Vector3 newPosition = new Vector3(columnIndex, row, 0f);

                // Instantiate + parent under levelRoot
                GameObject block = Instantiate(prefabToSpawn, newPosition, Quaternion.identity);
                block.transform.SetParent(levelRoot, true);
            }

            row++;
        }
    }

    // --------------------------------------------------------------------------
    void ReloadLevel()
    {
        if (levelRoot == null) return;

        foreach (Transform child in levelRoot)
            Destroy(child.gameObject);

        LoadLevel();
    }
}
