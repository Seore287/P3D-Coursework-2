using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string Name;

    public IEnumerator StartConversation()
    {
        // Placeholder for a conversation system, you can replace this with a full dialogue system
        Debug.Log("Starting conversation with " + Name);

        // Wait for a moment to simulate conversation
        yield return new WaitForSeconds(2f);

        Debug.Log("Conversation ended with " + Name);
    }
}
