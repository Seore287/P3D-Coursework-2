using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput input;

    [Header("Quest System Settings")]
    public QuestLogManager questLogManager;
    public QuestManager questManager;
    public QuestDialogueManager dialogueManager;
    public GameObject questDialogueBox;

    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    public QuestItem currentItem;
    private List<Quest> availableQuests;

    private void Awake()
    {
        input = new PlayerInput();
        input.Gameplay.Interact.performed += _ => ToggleDialogue();
        input.Gameplay.Pickup.performed += _ => PickupItem();

        input.Gameplay.ChooseQuest.performed += _ => HandleQuestSelection(0); // Choose Quest 1
        input.Gameplay.ChooseQuest2.performed += _ => HandleQuestSelection(1); // Choose Quest 2
        input.Gameplay.ChooseQuest3.performed += _ => HandleQuestSelection(2); // Choose Quest 3
        input.Gameplay.ChooseQuest4.performed += _ => HandleQuestSelection(3); // Choose Quest 4
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        if (questDialogueBox != null)
        {
            questDialogueBox.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Check if the player is near a quest item
            if (other.TryGetComponent<QuestItem>(out var questItem))
            {
                currentItem = questItem;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (questDialogueBox != null)
            {
                questDialogueBox.SetActive(false);
                isDialogueActive = false;
            }

            currentItem = null;
        }
    }

    private void ToggleDialogue()
    {
        if (!isPlayerInRange && !isDialogueActive)
        {
            if (questManager != null && dialogueManager != null)
            {
                availableQuests = questManager.GetAvailableQuests();

                if (availableQuests != null && availableQuests.Count > 0)
                {
                    string dialogueLine = "I have some quests available for you, young trailblazer. ";

                    // Add the quest names in a single line
                    for (int i = 0; i < availableQuests.Count; i++)
                    {
                        dialogueLine += $"{i + 1}. {availableQuests[i].QuestName}";
                        if (i < availableQuests.Count - 1)
                        {
                            dialogueLine += ", ";  
                        }
                    }

                    dialogueLine += ". Select a quest by pressing the corresponding number key.";

                    dialogueManager.StartDialogue(new string[] { dialogueLine });
                }
                else
                {
                    dialogueManager.StartDialogue(new string[]
                    {
                    "You have completed all the quests I have for you."
                    });
                }
            }

            isDialogueActive = true;
        }
        else if (isDialogueActive)
        {
            dialogueManager.ShowNextLine();
        }
    }

    private void PickupItem()
    {
        if (isPlayerInRange && currentItem != null)
        {
            // Mark the quest objective as completed
            questManager.CompleteQuestObjective(currentItem.ObjectiveID);
            Debug.Log($"Picked up item: {currentItem.name}");

            // Destroy or deactivate the item
            Destroy(currentItem.gameObject);
            currentItem = null;
        }
    }

    private void HandleQuestSelection(int questIndex)
    {
        if (isDialogueActive && availableQuests != null && availableQuests.Count > questIndex)
        {
            SelectQuest(availableQuests[questIndex]);
        }
    }

    private void SelectQuest(Quest quest)
    {
        if (quest != null)
        {
            questLogManager.AddQuest(quest);
            dialogueManager.StartDialogue(new string[]
            {
                $"You have accepted the quest: {quest.QuestName}!" 
                + " " + "Go now, Trailblazer. Your journey awaits.",
            });

            isDialogueActive = false; 
        }
    }
}
