using UnityEngine;

public class TriggerDialougeTest : MonoBehaviour
{
    public DialogueUIController dialogueUIController; // Reference to the DialogueUIController
    public DialogueLine dialogueLine; // Reference to the dialogue line to display

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (dialogueUIController != null && dialogueLine != null)
        {
            dialogueUIController.DisplayDialogueLine(dialogueLine);
        }
    }
}
