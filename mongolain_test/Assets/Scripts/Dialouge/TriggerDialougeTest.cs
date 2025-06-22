using UnityEngine;

public class TriggerDialogueTest : MonoBehaviour, IInteractable
{
    public DialogueUIController dialogueUIController; // Reference to the DialogueUIController
    public DialogueLine[] dialogueLines; // Reference to the dialogue lines to display

    private int currentDialogueIndex = 0;

    public void Interact()
    {
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        if (dialogueUIController != null && dialogueLines != null && dialogueLines.Length > 0)
        {
            if (currentDialogueIndex < dialogueLines.Length)
            {
                dialogueUIController.DisplayDialogueLine(dialogueLines[currentDialogueIndex]);
                currentDialogueIndex++;
            }
            else
            {
                Debug.Log("Dialogue has finished.");
                // Optionally, trigger an event or perform another action here
            }
        }
    }
}