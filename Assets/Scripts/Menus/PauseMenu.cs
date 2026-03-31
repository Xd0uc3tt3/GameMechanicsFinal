using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject controlsPanel;

    public void TogglePause()
    {
        if (IsPaused)
        {
            Resume();
        }

        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        IsPaused = true;
        pauseMenuCanvas.SetActive(true);
        controlsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void Resume()
    {
        IsPaused = false;
        pauseMenuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
