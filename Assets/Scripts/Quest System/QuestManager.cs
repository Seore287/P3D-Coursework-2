using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Quest References")]
    [SerializeField] private QuestLogManager questLogManager;
    [SerializeField] private PlayerStats playerStats;

    [Header("UI References")]
    [SerializeField] private TMP_Text xpText;

    public List<Quest> quests = new();

    public List<Quest> GetAvailableQuests()
    {
        // Return quests that are not completed or in progress
        return quests.FindAll(q => !q.isCompleted && q.Progress < 100f);
    }

    public Quest GetAvailableQuest()
    {
        // Return the first available quest
        return quests.Find(q => !q.isCompleted && q.Progress < 100f);
    }

    public void StartQuest(Quest quest)
    {
        if (quest != null && quest.Progress == 0f)
        {
            quest.BeginQuest();
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest != null && !quest.isCompleted && quest.Progress >= 100f)
        {
            quest.CompleteQuest();
            AwardXP(quest.XP);
        }
    }

    private void AwardXP(int xp)
    {
        if (playerStats != null)
        {
            playerStats.AddXP(xp);
            UpdateXPText();
        }
    }

    private void UpdateXPText()
    {
        if (xpText != null && playerStats != null)
        {
            xpText.text = $"XP: {playerStats.CurrentXP}/{playerStats.MaxXP}";
        }
    }

    public void CompleteQuestObjective(string objectiveID)
    {
        foreach (var quest in questLogManager.activeQuests)
        {
            foreach (var objective in quest.Objectives)
            {
                if (!objective.isCompleted && objective.Tasks == objectiveID)
                {
                    objective.CompleteObjective();
                    quest.UpdateQuestProgress();
                    Debug.Log($"Objective '{objectiveID}' completed!");
                    return;
                }
            }
        }
        Debug.Log($"No matching objective found for ObjectiveID: {objectiveID}");
    }


    public void UpdateQuestObjective(Quest quest, QuestObjective objective)
    {
        if (quest != null && objective != null && quest.Objectives.Contains(objective))
        {
            objective.CompleteObjective();
            quest.UpdateQuestProgress();
        }
    }
}
