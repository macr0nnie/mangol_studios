using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueLine))]
public class DialogueLineEditor : Editor
{
    // this script is used to create a custom editor in the unity menu for esme and tia
    //they can directly write the lines
    SerializedProperty dialogueText;
    SerializedProperty voiceLine;
    SerializedProperty displayType;
    SerializedProperty image;
    SerializedProperty choices;

    private void OnEnable()
    {
        // Cache references to the properties in DialogueLine
        dialogueText = serializedObject.FindProperty("dialogueText");
        voiceLine = serializedObject.FindProperty("voiceLine");
        displayType = serializedObject.FindProperty("displayType");
        image = serializedObject.FindProperty("image");
        choices = serializedObject.FindProperty("choices");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the basic fields
        EditorGUILayout.PropertyField(dialogueText);
        EditorGUILayout.PropertyField(voiceLine);
        EditorGUILayout.PropertyField(displayType);
        EditorGUILayout.PropertyField(image);

        // Draw the choices list
        EditorGUILayout.LabelField("Choices", EditorStyles.boldLabel);
        for (int i = 0; i < choices.arraySize; i++)
        {
            SerializedProperty choiceProp = choices.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(choiceProp, new GUIContent($"Choice {i + 1}"));
        }

        if (GUILayout.Button("Add Choice"))
        {
            choices.InsertArrayElementAtIndex(choices.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
