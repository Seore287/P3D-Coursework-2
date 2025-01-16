using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;           // Text field for dialogue
    public GameObject questDialoguePanel;   // Panel for quest-specific dialogue
    public Button nextButton;               // Button to go to the next dialogue line

    [Header("Settings")]
    public float typingSpeed = 0.05f;       // Speed of text typing effect

    private Queue<string> dialogueLines = new Queue<string>(); // Queue to store dialogue lines
    private bool isTyping = false;          // Tracks if text is being typed

    private void Start()
    {
        // Ensure the dialogue panel and button are initially inactive
        if (questDialoguePanel != null)
        {
            questDialoguePanel.SetActive(false);
        }

        if (dialogueText != null)
        {
            dialogueText.text = ""; // Clear any placeholder text
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false); // Hide the button initially
            nextButton.onClick.AddListener(ShowNextLine); // Assign button functionality
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

        // Activate the dialogue panel and button
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

        ShowNextLine(); // Display the first line
    }

    // Show the next line of dialogue
    public void ShowNextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();  // Stop typing if currently in progress
            dialogueText.text = dialogueLines.Peek(); // Immediately display the current line
            isTyping = false;    // Mark typing as complete
        }
        else if (dialogueLines.Count > 0)
        {
            string nextLine = dialogueLines.Dequeue(); // Get the next line
            StartCoroutine(TypeText(nextLine));        // Start typing the line
        }
        else
        {
            EndDialogue(); // No more lines, end the dialogue
        }
    }

    // Coroutine to type out text letter by letter
    private IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear current text

        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter; // Add each letter
            yield return new WaitForSeconds(typingSpeed); // Wait before the next letter
        }

        isTyping = false; // Typing is complete
    }

    // End the dialogue and deactivate UI elements
    private void EndDialogue()
    {
        if (questDialoguePanel != null)
        {
            questDialoguePanel.SetActive(false); // Deactivate the panel
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false); // Deactivate the button
        }

        dialogueText.text = ""; // Clear the text

        // Deactivate the cursor after the dialogue ends
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
