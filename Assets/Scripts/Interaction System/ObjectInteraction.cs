using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Object Interaction Settings")]
    public string interactionPrompt = "Press E to interact with the object"; 
    private bool isPlayerNearby = false;  
    private bool isInteracting = false;  

    [Header("UI Settings")]
    public GameObject[] uiPrompts;  
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Gameplay.Interact.performed += _ => InteractWithObject();
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
        // Ensure the UI prompts are hidden at the start
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

    private void InteractWithObject()
    {
        // Check if the player is nearby and ready to interact
        if (isPlayerNearby && !isInteracting)
        {
            isInteracting = true;
            Debug.Log("Interacting with object: " + interactionPrompt);
            PerformObjectInteraction();  // Perform the actual object interaction (customizable)
            DisplayUIPrompt(false); // Hide the prompt after interaction starts
        }
        else
        {
            Debug.Log("Cannot interact. Player is not nearby.");
        }
    }

    private void PerformObjectInteraction()
    {
        // Example interaction logic (e.g., open a door, pick up an item, etc.)
        // You can customize this to fit your needs.
        Debug.Log("Object interaction performed!");
    }

    private void DisplayUIPrompt(bool showPrompt)
    {
        // Show or hide the UI prompts based on player proximity
        if (uiPrompts != null)
        {
            foreach (GameObject prompt in uiPrompts)
            {
                prompt.SetActive(showPrompt);  // Show or hide each prompt
            }
        }
    }
}
