using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text rewardText;

    public void ShowRewardMessage(string message)
    {
        rewardText.text = message;
        rewardText.gameObject.SetActive(true);
        Invoke(nameof(HideRewardMessage), 5f); // Hide after 5 seconds
    }

    private void HideRewardMessage()
    {
        rewardText.gameObject.SetActive(false);
    }
}
