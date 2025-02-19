using UnityEngine;

public class TriggerDialougeTest : MonoBehaviour
{
    public DialogueUIController dialogueUIController; // Reference to the DialogueUIController
    public DialogueLine[] dialogueLines; // Reference to the dialogue lines to display

    private int currentDialogueIndex = 0;

    void Update()
    {
        //just to test the dialogue system
        if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
        {
            TriggerDialogue();
        }
        //in the future this will be triggered by the game events 
    }
    private void TriggerDialogue()
    {
        if (dialogueUIController != null && dialogueLines != null && dialogueLines.Length > 0)
        {
            dialogueUIController.DisplayDialogueLine(dialogueLines[currentDialogueIndex]);
            currentDialogueIndex = (currentDialogueIndex + 1) % dialogueLines.Length;
        }
    }
}