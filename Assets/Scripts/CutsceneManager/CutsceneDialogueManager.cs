using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneDialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;   
    public Button nextButton;       
    public float typingSpeed = 0.05f;

    [TextArea (4 , 15)]
    public string[] dialogueLines;

    private bool isDisplaying = false;
    private int currentCutsceneLine = 0;  

    private void Start()
    {
        nextButton.gameObject.SetActive(false);  
        StartCoroutine(DisplayDialogue(dialogueLines[currentCutsceneLine]));  
        nextButton.onClick.AddListener(OnButtonClicked);  
    }

    public IEnumerator DisplayDialogue(string dialogue)
    {
        if (isDisplaying) yield break;  

        isDisplaying = true;
        dialogueText.text = ""; 

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        nextButton.gameObject.SetActive(true);
        isDisplaying = false;
    }

    public void OnButtonClicked()
    {
        currentCutsceneLine++;

        if (currentCutsceneLine < dialogueLines.Length)  
        {
            nextButton.gameObject.SetActive(false);  
            StartCoroutine(DisplayDialogue(dialogueLines[currentCutsceneLine]));  
        }
        else
        {
            EndCutscene(); 
        }
    }

    private void EndCutscene()
    {
        //trigger the next scene
        dialogueText.text = "";  
        nextButton.gameObject.SetActive(false);  
    }
}
