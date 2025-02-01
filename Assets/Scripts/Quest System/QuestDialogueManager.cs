using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;           
    public GameObject questDialoguePanel;   
    public Button nextButton;              

    [Header("Settings")]
    public float typingSpeed = 0.05f;   

    private Queue<string> dialogueLines = new Queue<string>();
    private bool isTyping = false;   

    private void Start()
    {
        if (questDialoguePanel != null)
        {
            questDialoguePanel.SetActive(false);
        }

        if (dialogueText != null)
        {
            dialogueText.text = ""; 
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false); 
            nextButton.onClick.AddListener(ShowNextLine); 
        }
    }

    // Start a new dialogue sequence
    public void StartDialogue(string[] lines)
    {
        // Clear any existing lines and add new ones
        dialogueLines.Clear();
        foreach (string line in lines)
        {
            dialogueLines.Enqueue(line);
        }

        if (questDialoguePanel != null)
        {
            questDialoguePanel.SetActive(true);
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(true);
        }

        // Activate the cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ShowNextLine(); 
    }

    public void ShowNextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();  
            dialogueText.text = dialogueLines.Peek(); 
            isTyping = false;   
        }
        else if (dialogueLines.Count > 0)
        {
            string nextLine = dialogueLines.Dequeue(); 
            StartCoroutine(TypeText(nextLine));        
        }
        else
        {
            EndDialogue(); 
        }
    }

    // Coroutine to type out text letter by letter
    private IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = ""; 

        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }

        isTyping = false; 
    }

    private void EndDialogue()
    {
        if (questDialoguePanel != null)
        {
            questDialoguePanel.SetActive(false); 
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false); 
        }

        dialogueText.text = ""; 

        // Deactivate the cursor after the dialogue ends
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
