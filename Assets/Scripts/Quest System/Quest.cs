using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]  
public class Quest : ScriptableObject
{
    public string QuestName;
    [TextArea(4, 8)]
    public string QuestDescription;
    public List<QuestObjective> Objectives;
    public int XP;
    public bool isCompleted;
    public float Progress;

    public void CompleteQuest()
    {
        isCompleted = true;
        Progress = 100f;

    }

    public void BeginQuest()
    {
        isCompleted = false;
        UpdateQuestProgress();
        
    }

    public void UpdateQuestProgress()
    {
        if (Objectives.Count == 0)
        {
            Progress = 100f;
            return;
        }

        int completedCount = 0;
        foreach (var objective in Objectives)
        {
            if (objective.isCompleted) completedCount++;
        }

        Progress = (completedCount / (float)Objectives.Count) * 100f;
    }
}        

[System.Serializable]  
public class QuestObjective
{
    
    [TextArea(2, 4)]
    public string Tasks;
    public bool isCompleted;

    public QuestObjective(string description)
    {
        Tasks = description;
        isCompleted = false;
    }

    public void CompleteObjective()
    {
        isCompleted = true;
    }
}
