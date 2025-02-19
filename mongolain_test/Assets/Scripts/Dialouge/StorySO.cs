using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Story", menuName = "Dialogue/CreateStory")]
public class DialogueSequence : ScriptableObject
{
    //just contains a list of scriptable objects for dialoge lines needs to add more functionality
    public List<DialogueLine> dialogueLines; // List of dialogue lines in the sequence

    //in the future create an editor script to add more functionality
}

