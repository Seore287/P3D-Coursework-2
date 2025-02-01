using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Dialogue References")]
    public DialogueLine[] dialogueLines; 
    private DialogueManager dialogueManager; 
    private bool isPlayerNearby = false;   
    private bool isDialogueActive = false;  

    [Header("UI Settings")]
    public GameObject[] uiPrompts;  
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Gameplay.NPCInteract.performed += _ => InteractWithNPC();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene!");
        }

        if (uiPrompts != null)
        {
            foreach (GameObject prompt in uiPrompts)
            {
                prompt.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the interaction range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            DisplayUIPrompt(true);  // Show the UI prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the interaction range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            DisplayUIPrompt(false);  // Hide the UI prompt
        }
    }

    public void InteractWithNPC()
    {
        // Check if the player is nearby and ready to interact
        if (isPlayerNearby)
        {
            if (!isDialogueActive)
            {
                // Start dialogue if it's not already active
                dialogueManager.StartDialogue(dialogueLines);
                isDialogueActive = true;
                DisplayUIPrompt(false); // Hide the prompt once interaction begins
            }
            else
            {
                // Show next line of dialogue if it's active
                dialogueManager.ShowNextLine();
            }
        }
        else
        {
            Debug.Log("Cannot interact. Player is not nearby.");
        }
    }

    private void DisplayUIPrompt(bool showPrompt)
    {
        // Show or hide the UI prompt based on player proximity
        if (showPrompt) 
        {
            foreach (GameObject prompt in uiPrompts) 
            {
                prompt.SetActive(true);
            }
        }
        else 
        {
            foreach (GameObject prompt in uiPrompts) 
            {
                prompt.SetActive(false);
            }
        }
    }
}
