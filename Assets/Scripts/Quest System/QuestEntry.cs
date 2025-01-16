using UnityEngine;

public class QuestEntry : MonoBehaviour
{
    public Quest quest;
    public QuestLogManager questLogManager;

    public void OnQuestSelected()
    {
        questLogManager.selectedQuest = quest;
        questLogManager.UpdateQuestLogUI();
    }

    public void OnQuestCompleted()
    {
        questLogManager.MarkQuestAsComplete(quest);
    }
}
