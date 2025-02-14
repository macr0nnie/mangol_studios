using UnityEngine;

public enum DialogueDisplayType
{
    Standard,   // Regular text bubble
    Cutscene    // Larger panel with an image
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;          // Text for this choice
    public DialogueLine nextDialogue;  // The dialogue line that follows if this choice is selected
}
