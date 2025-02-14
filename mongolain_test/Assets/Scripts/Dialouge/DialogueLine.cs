using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueLine", menuName = "Dialogue/DialogueLine")]
public class DialogueLine : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogueText;                // The dialogue text
    public AudioClip voiceLine;                // Optional voice clip for this line
    public DialogueDisplayType displayType;    // How to display this line (Standard or Cutscene)
    public Sprite image;                       // Image for cutscene display (e.g., portrait or background)
    public List<DialogueChoice> choices = new List<DialogueChoice>();  // Branching choices, if any
}
