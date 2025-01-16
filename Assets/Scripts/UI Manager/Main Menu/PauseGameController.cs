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
        playerInput.UI.Pause.performed += _ => TogglePauseMenu();
    }

    private void OnEnable()
    {
        
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false; // Hide cursor when resuming the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true; // Show cursor in pause menu
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    public void ShowControlsPanel()
    {
        PlayerPrefs.SetInt("OpenControlsPanel", 1);
        BackToMainMenu();
    }

    public void BackToPauseMenu()
    {
        pauseMenu.SetActive(true); // Show the Pause menu

        Cursor.visible = true; // Ensure cursor is visible
        Cursor.lockState = CursorLockMode.None;
    }
}
