using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//to-do 
//refactor the code to support three panels
public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue Panels")]
    [SerializeField] private GameObject cutscenePanel;     // Full screen cutscene panel
    [SerializeField] private GameObject topScreenPanel;    // Top screen text panel
    [SerializeField] private GameObject bubblesPanel;      // Speech bubble panel

    [Header("Text Components")]
    [SerializeField] private TMP_Text cutsceneText;        // Text for cutscene panel
    [SerializeField] private TMP_Text topScreenText;       // Text for top screen panel
    [SerializeField] private TMP_Text bubblesText;         // Text for speech bubbles

    [Header("Image Components")]
    [SerializeField] private Image dialogueSpeakerImage;   // Speaker portrait
    [SerializeField] private Image fullScreenImage;        // Background image for cutscenes

    [Header("Choice Panel Components")]
    [SerializeField] private GameObject choicePanel;       // Panel containing choice buttons
    [SerializeField] private List<Button> choiceButtons;

    //typing effect
    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;    // Speed of the typing effect
    private Coroutine typingCoroutine;               // Reference to the current typing coroutine
    private DialogueLine currentDialogueLine;  // The current dialogue line being shown

    public void Start()
    {
        cutscenePanel.SetActive(false);
        bubblesPanel.SetActive(false);
        choicePanel.SetActive(false);
        topScreenPanel.SetActive(false);
    }
    public void DisplayDialogueLine(DialogueLine line)
    {
        currentDialogueLine = line;

        cutscenePanel.SetActive(false);
        bubblesPanel.SetActive(false);
        topScreenPanel.SetActive(false);

        switch (line.displayType)
        {
            case DialogueDisplayType.Standard:
                topScreenPanel.SetActive(true);
                topScreenText.text = line.dialogueText; //use the text effect
                break;
            case DialogueDisplayType.Cutscene:
                cutscenePanel.SetActive(true);
                cutsceneText.text = line.dialogueText;
                break;
            case DialogueDisplayType.SpeachBubble:
                bubblesPanel.SetActive(true);
                bubblesText.text = line.dialogueText;
                break;
        }

    }
    private void DisplayChoices(List<DialogueChoice> choices)
    {
        choicePanel.SetActive(true);

        // Loop through pre-assigned buttons and set them up with choices
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < choices.Count)
            {
                Button btn = choiceButtons[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;

                // Clear previous listeners and assign a new one
                btn.onClick.RemoveAllListeners();
                int choiceIndex = i; // Capture current index for the lambda
                btn.onClick.AddListener(() => OnChoiceSelected(choices[choiceIndex]));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
    private void HideChoices()
    {
        choicePanel.SetActive(false);
        foreach (Button btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
    }

    private void OnChoiceSelected(DialogueChoice selectedChoice)
    {
        HideChoices();

        if (selectedChoice.nextDialogue != null)
        {
            // Continue with the next dialogue line if one is assigned
            DisplayDialogueLine(selectedChoice.nextDialogue);
        }
    }
    private IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        textComponent.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter; // Add one letter at a time
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                textComponent.text = fullText;
                break;
            }
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified typing speed
        }
    }
    public void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            // Display the full text immediately
            switch (currentDialogueLine.displayType)
            {
                case DialogueDisplayType.Standard:
                    topScreenText.text = currentDialogueLine.dialogueText;
                    break;
                case DialogueDisplayType.Cutscene:
                    cutsceneText.text = currentDialogueLine.dialogueText;
                    break;
                case DialogueDisplayType.SpeachBubble:
                    bubblesText.text = currentDialogueLine.dialogueText;
                    break;
            }
        }
    }


}
