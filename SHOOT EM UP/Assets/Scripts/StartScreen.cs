using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [Tooltip("Panel GameObject that contains the start table UI (this GameObject can be the panel itself).")]
    public GameObject panel;
    public float waitSeconds = 3f;

    bool started = false;

    void Start()
    {
        if (panel == null) panel = gameObject;
        panel.SetActive(true);
        // pause game time for gameplay coroutines that depend on Time.timeScale
        Time.timeScale = 0f;
        StartCoroutine(AutoHide());
    }

    IEnumerator AutoHide()
    {
        yield return new WaitForSecondsRealtime(waitSeconds);
        HideAndResume();
    }

    void Update()
    {
        if (started) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            HideAndResume();
        }
    }

    void HideAndResume()
    {
        if (started) return;
        started = true;
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}