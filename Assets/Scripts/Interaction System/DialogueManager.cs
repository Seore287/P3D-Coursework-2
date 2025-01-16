using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text characterNameText;
    public Image characterImage;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public Button nextButton;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;

    private Queue<DialogueLine> dialogueLines = new Queue<DialogueLine>();
    private bool isDialogueActive = false;
    private bool isTyping = false;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        dialogueLines.Clear();

        foreach (DialogueLine line in lines)
        {
            dialogueLines.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        isDialogueActive = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        nextButton.gameObject.SetActive(true);
        ShowNextLine();
    }

    public void OnButtonClicked()
    {
        if (!isTyping)
        {
            ShowNextLine();
        }
    }

    public void ShowNextLine()
    {
        if (dialogueLines.Count > 0)
        {
            DialogueLine currentLine = dialogueLines.Dequeue();

            characterNameText.text = currentLine.CharacterName;
            characterImage.sprite = currentLine.CharacterImage;

            nextButton.gameObject.SetActive(false);
            StartCoroutine(TypeText(currentLine.dialogueText));
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        nextButton.gameObject.SetActive(true);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void DisplayLine(DialogueLine line)
    {
        characterNameText.text = line.CharacterName;
        characterImage.sprite = line.CharacterImage;
        dialogueText.text = line.dialogueText;

        nextButton.gameObject.SetActive(true);
    }
}