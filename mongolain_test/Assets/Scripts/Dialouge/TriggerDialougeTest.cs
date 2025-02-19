using UnityEngine;

public class TriggerDialogueTest : MonoBehaviour
{
    public DialogueUIController dialogueUIController; // Reference to the DialogueUIController
    public DialogueLine[] dialogueLines; // Reference to the dialogue lines to display

    private int currentDialogueIndex = 0;

    void Update()
    {
        // Just to test the dialogue system
        if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
        {
            TriggerDialogue();
        }
        // In the future, this will be triggered by the game events 
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