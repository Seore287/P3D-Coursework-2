using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen; 
    [SerializeField] private TMP_Text loadingText; 
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private AudioSource background;

    [SerializeField] private float fadeDuration = 2f;

    public void LoadScene(string nameOfScene)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        StartCoroutine(FadeLoadScene(nameOfScene));
    }

    private IEnumerator FadeLoadScene(string nameOfScene)
    {
        // Start fade-out effect
        yield return StartCoroutine(Fade(1f));

        AsyncOperation operation = SceneManager.LoadSceneAsync(nameOfScene);
        operation.allowSceneActivation = false;

        float progress = 0f;

        while (!operation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);

            while (progress < loadingProgress)
            {
                progress += Time.deltaTime;
                UpdateLoading(progress);
                yield return null;
            }

            if (operation.progress >= 0.9f)
            {
                while (progress < 1f)
                {
                    progress += Time.deltaTime;
                    UpdateLoading(progress);
                    yield return null;
                }

                yield return new WaitForSeconds(1.5f);

                // Start fade-in effect
                yield return StartCoroutine(Fade(0f));
                operation.allowSceneActivation = true;

                if (background != null)
                {
                    // Optionally stop the background music for the cutscene
                    background.Stop();

                    // Transition to new music or modify the background music here if needed
                }
            }

            yield return null;
        }
    }

    private IEnumerator Fade(float targetValue)
    {
        if (fadeCanvas == null) yield break;

        float startAlpha = fadeCanvas.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetValue, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = targetValue;
    }

    private void UpdateLoading(float progress)
    {
        if (loadingText != null)
        {
            loadingText.text = $"Loading... {Mathf.FloorToInt(progress * 100)}%";
        }
    }
}
