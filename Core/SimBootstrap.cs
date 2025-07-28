using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[AddComponentMenu("Simulation/Sim Bootstrap")]
public sealed class SimBootstrap : MonoBehaviour
{
    [Tooltip("Seconds per physics tick")]
    [SerializeField] float fixedTimeStep = 0.01f;

    bool paused;

    void Awake()
    {
        Time.fixedDeltaTime = fixedTimeStep;
        Application.runInBackground = true;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Toggle pause with Space
        if (kb.spaceKey.wasPressedThisFrame)
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
        }

        // Step one physics tick when paused
        if (paused && kb.rightArrowKey.wasPressedThisFrame)
            StartCoroutine(StepOnce());

        // Reset scene with R
        if (kb.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    System.Collections.IEnumerator StepOnce()
    {
        Time.timeScale = 1f;
        yield return new WaitForFixedUpdate();
        yield return null;
        Time.timeScale = 0f;
    }
}
