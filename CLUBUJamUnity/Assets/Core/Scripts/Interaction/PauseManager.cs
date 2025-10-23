using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{

    public static PauseManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;
    private bool isPaused = false;

    void Start()
    {
        EventHolder.Instance.onPause.AddListener(PausePerformed);
        EventHolder.Instance.onExitUI.AddListener(ExitPause);
    }

    private void OnEnable()
    {
        if (EventHolder.Instance != null)
            EventHolder.Instance.onInteract.AddListener(PausePerformed);
    }

    private void OnDisable()
    {
        if (EventHolder.Instance != null)
            EventHolder.Instance.onInteract.RemoveListener(PausePerformed);
    }

    public void PausePerformed()
    {
        if (isPaused) 
            Resume();
        else
            Pause();
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        InteractionManager.Instance.SwitchToGameplay();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        InteractionManager.Instance.SwitchToUI();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void ExitPause()
    {
        Resume();
    }
}
