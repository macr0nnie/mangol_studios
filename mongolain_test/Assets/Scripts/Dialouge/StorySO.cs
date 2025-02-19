using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Story", menuName = "Dialogue/CreateStory")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> dialogueLines; // List of dialogue lines in the sequence
}


[CustomEditor(typeof(DialogueSequence))]
public class DialogueSequenceEditor : Editor
{
    SerializedProperty dialogueLines;

    private void OnEnable()
    {
        dialogueLines = serializedObject.FindProperty("dialogueLines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(dialogueLines, new GUIContent("Dialogue Lines"), true);

        if (GUILayout.Button("Add Dialogue Line"))
        {
            AddDialogueLine();
        }

        serializedObject.ApplyModifiedProperties();

        // Display and edit each dialogue line
        for (int i = 0; i < dialogueLines.arraySize; i++)
        {
            SerializedProperty dialogueLineProp = dialogueLines.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(dialogueLineProp, new GUIContent($"Dialogue Line {i + 1}"), true);

            if (GUILayout.Button("Remove Dialogue Line"))
            {
                RemoveDialogueLine(i);
            }
        }
    }

    private void AddDialogueLine()
    {
        DialogueLine newDialogueLine = CreateInstance<DialogueLine>();
        newDialogueLine.name = "New Dialogue Line";
        AssetDatabase.AddObjectToAsset(newDialogueLine, target);
        AssetDatabase.SaveAssets();
        dialogueLines.arraySize++;
        dialogueLines.GetArrayElementAtIndex(dialogueLines.arraySize - 1).objectReferenceValue = newDialogueLine;
    }

    private void RemoveDialogueLine(int index)
    {
        SerializedProperty dialogueLineProp = dialogueLines.GetArrayElementAtIndex(index);
        Object dialogueLineObj = dialogueLineProp.objectReferenceValue;
        dialogueLineProp.objectReferenceValue = null;
        dialogueLines.DeleteArrayElementAtIndex(index);
        if (dialogueLineObj != null)
        {
            DestroyImmediate(dialogueLineObj, true);
        }
        AssetDatabase.SaveAssets();
    }
}