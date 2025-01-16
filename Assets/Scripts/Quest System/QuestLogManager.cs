using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogManager : MonoBehaviour
{
    [Header("Quest Log Manager References")]
    public GameObject questLogPanel;
    public Transform questLogContainer;
    public TMP_Text questEntryDetails;

    [Header("Active Quest References")]
    public List<Quest> activeQuests = new();
    public Quest selectedQuest;
    
    private PlayerInput input;

    private void Awake()
    {
        input = new PlayerInput();
        input.Gameplay.OpenQuestLog.performed += _ => ToggleQuestLog();
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
        if (questLogPanel != null)
        {
            questLogPanel.SetActive(false);  
        }

        if (questEntryDetails != null)
        {
            questEntryDetails.gameObject.SetActive(false);  
        }
    }

    private void ToggleQuestLog()
    {
        if (questLogPanel != null)
        {
            bool isActive = questLogPanel.activeSelf;
            questLogPanel.SetActive(!isActive); 

            if (!isActive)
            {
                UpdateQuestLogUI();
            }

            if (questEntryDetails != null)
            {
                questEntryDetails.gameObject.SetActive(!isActive);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void UpdateQuestLogUI()
    {
        if (questEntryDetails != null)
        {
            if (selectedQuest != null)
            {
                questEntryDetails.text = selectedQuest.QuestName + "\n" + selectedQuest.QuestDescription + " \n";

                foreach (var objective in selectedQuest.Objectives)
                {
                    questEntryDetails.text += $"{objective.Tasks}\n " + (objective.isCompleted ? "[Completed] " : "[Incomplete] ");
                }

                float progress = selectedQuest.Progress;
                questEntryDetails.text += $"\nProgress: {progress:F1}%";
            }
            else
            {
                questEntryDetails.text = "No quest selected.";
            }
        }
    }


    public void AddQuest(Quest newQuest)
    {
        if (newQuest != null)
        {
            activeQuests.Add(newQuest);
            selectedQuest = newQuest;  
            UpdateQuestLogUI();
        }
    }

    public void MarkQuestAsComplete(Quest quest)
    {
        if (quest != null && activeQuests.Contains(quest))
        {
            quest.CompleteQuest();
            UpdateQuestLogUI();
        }
    }
}
