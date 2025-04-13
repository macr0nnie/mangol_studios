using UnityEngine;
using UnityEngine.UIElements;

public class NPC_Trigger : MonoBehaviour, IInteractable

{
   // [SerializeField] private Dialogue dialogue; // Reference to the Dialogue scriptable object

    public void Interact()
    {
        // Trigger the dialogue when the player interacts with the NPC
       // DialogueManager.Instance.StartDialogue(dialogue);
    }
}
