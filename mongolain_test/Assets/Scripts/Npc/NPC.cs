using UnityEngine;

public class NPC : MonoBehaviour
{
    public string name;
    public DialogueLine[] dialogueLines;
    public float interactionDistance = 2f; // Distance within which the player can interact with the NPC
    public DialougeLine currentDialogueLine;

    //get the current story stage
    public void Awake()
    {
        //initialize values
    }
    public void UpdateDialouge(int stage)
    {
        switch (stage)
        {
            case 0:
                // Set the NPC's dialogue for stage 0
                currentDialogueLine = dialogueLines[0];

                break;
            case 1:
                // Set the NPC's dialogue for stage 1
                currentDialogueLine = dialogueLines[1];
                break;
            case 2:
                // Set the NPC's dialogue for stage 2
                currentDialogueLine = dialogueLines[2];
                break;
            default:
                Debug.LogWarning("Invalid story stage");
                break;
        }
    }
}
