using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string ObjectiveID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<QuestManager>(out var questManager))
            {
                // Attempt to complete the objective using the ObjectiveID
                questManager.CompleteQuestObjective(ObjectiveID);

                // Destroy the item or deactivate it after pickup
                Destroy(gameObject);
            }
        }
    }
}
