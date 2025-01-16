using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string CharacterName;
    public Sprite CharacterImage;
    [TextArea(5, 8)]
    public string dialogueText;
}