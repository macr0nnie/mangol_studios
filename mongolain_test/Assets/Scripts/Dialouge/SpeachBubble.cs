using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeachBubble : MonoBehaviour
{
    public DialogueLine dialogueLine; 
    public float displayDuration = 5f; // Duration to display the bubble
    private void Start()
    {
        // Start the coroutine to display the speech bubble
        StartCoroutine(DisplaySpeechBubble());
    }
    private IEnumerator DisplaySpeechBubble()
    {
        // Display the dialogue text (you can replace this with your own UI logic)
        Debug.Log(dialogueLine.dialogueText);
        // Wait for the specified duration
        yield return new WaitForSeconds(displayDuration);
        // Hide or destroy the speech bubble (you can replace this with your own UI logic)
        Destroy(gameObject);
    }
    //tell camera to zoom in on player and bubble
    //go back to main when the player walks away.
}