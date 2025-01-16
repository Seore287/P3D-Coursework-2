using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string cutScene;
    public GameObject optionsScreen;
    public GameObject controlsScreen;
    public LoadingManager LoadingManager;

    public AudioSource buttonClick;
    public AudioSource mainmenuMusic;  
    public AudioSource cutsceneMusic;

    [SerializeField] private float musicFade = 1f;
    
    public void Start()
    {
        // Ensure the background music continues playing across scenes
        if (mainmenuMusic != null)
        {
            DontDestroyOnLoad(mainmenuMusic.gameObject);
        }

        // Ensure cutscene music is not playing initially
        if (cutsceneMusic != null)
        {
            cutsceneMusic.Stop();
        }

        // Handle Controls Screen logic based on PlayerPrefs
        if (PlayerPrefs.GetInt("OpenControlsPanel", 0) == 1)
        {
            PlayerPrefs.SetInt("OpenControlsPanel", 0); // Reset the flag
            OpenControlsPanel(); // Open controls panel
        }
        else
        {
            controlsScreen.SetActive(false);
        }

        controlsScreen.SetActive(false);
    }
    public void StartGame()
    {
        ActivateSound();

        if (LoadingManager != null)
        {
            StartCoroutine(TransitionSound());
            LoadingManager.LoadScene(cutScene);
        }
    }
    public void OpenOptions()
    {
        ActivateSound();
        optionsScreen.SetActive(true);
    }
    public void CloseOptions()
    {
        ActivateSound();
        optionsScreen.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        ActivateSound();
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(true); // Activate the controls panel
        }
    }

    public void CloseControlsPanel()
    {
        ActivateSound();
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(false); // Deactivate the controls panel
        }
    }

    private void ActivateSound()
    {
        if (buttonClick != null)
        {
            buttonClick.Play();
        }
    }

    private IEnumerator TransitionSound()
    {
        if (mainmenuMusic != null)
        {
            float startVolume = mainmenuMusic.volume;

            // Fade out
            for (float t = 0; t < musicFade; t += Time.deltaTime)
            {
                mainmenuMusic.volume = Mathf.Lerp(startVolume, 0, t / musicFade);
                yield return null;
            }

            // Ensure music is completely muted after the fade-out
            mainmenuMusic.volume = 0;
            mainmenuMusic.Stop();
        }

        // Fade in the cutscene music
        if (cutsceneMusic != null)
        {
            cutsceneMusic.Play();
            float startVolume = 0f;
            cutsceneMusic.volume = startVolume;

            // Fade in
            for (float t = 0; t < musicFade; t += Time.deltaTime)
            {
                cutsceneMusic.volume = Mathf.Lerp(startVolume, 1, t / musicFade);
                yield return null;
            }

            cutsceneMusic.volume = 1;
        }
    }

    public void ExitGame()
    {
        ActivateSound();
        Application.Quit();
    }
}
