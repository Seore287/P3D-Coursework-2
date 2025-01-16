using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseGameController : MonoBehaviour
{
    private PlayerInput playerInput;

    [Header("Pause Settings")]
    public GameObject pauseMenu;
    public bool isPaused;
    public string MainMenuScene;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.UI.Pause.performed += TogglePauseMenu;
        playerInput.UI.Enable();
    }

    private void OnDisable()
    {
        playerInput.UI.Pause.performed -= TogglePauseMenu;
        playerInput.UI.Disable();
    }
    private void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        } else
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }
}
