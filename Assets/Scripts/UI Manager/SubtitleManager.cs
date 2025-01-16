using System.Collections;
using UnityEngine;
using TMPro; 
public class SubtitleManager : MonoBehaviour
{
    public TMP_Text subtitleText;

    public void ShowSubtitle(string text)
    {
        Debug.Log("ShowSubtitle called with argument: " + text);

        StopAllCoroutines();
        StartCoroutine(DisplaySubtitle(text));
    }

    private IEnumerator DisplaySubtitle(string text)
    {
        Debug.Log("Displaying subtitle: " + text);
        subtitleText.text = text;
        subtitleText.enabled = true; // Ensure this is enabled
        yield return new WaitForSeconds(3f); // Default duration of 3 seconds
        subtitleText.text = "";
        subtitleText.enabled = false;
        Debug.Log("Subtitle cleared.");
    }
}
