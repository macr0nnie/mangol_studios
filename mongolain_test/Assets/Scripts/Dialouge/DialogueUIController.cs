using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue Panel Components")]

    public GameObject cutscene_panel;          // The panel containing the cutscene
    //cutscene text
    public TMP_Text cutsceneText;
   
    public GameObject bubbles_panel;
    public TMP_Text bubblesText;
    public Image dialogueImage;                // Image element for cutscene display
    

    public RectTransform dialoguePanel;      // The panel containing the dialogue
    public TextMeshProUGUI dialogueText;       // Text element for dialogue text
                 // Image element for cutscene display

    [Header("Choice Panel Components")]
    public GameObject choicePanel;             // Panel that contains the choice buttons
    public List<Button> choiceButtons;         // Pre-assigned buttons to display choices
    private DialogueLine currentDialogueLine;  // The current dialogue line being shown
    /// <summary>
    /// Displays a dialogue line on the UI.
    /// </summary>
    public void DisplayDialogueLine(DialogueLine line)
    {
        currentDialogueLine = line;
        // Set up the panel based on the display type
        if (line.displayType == DialogueDisplayType.Cutscene)
        {
            //enable the cutscene panel
            cutscene_panel.SetActive(true);
           //set the cutscene image
           //need to be able to change the image depending on the dialoge line? in future
            dialogueImage.sprite = line.image; 
        }
        else
        {
            bubbles_panel.SetActive(true);
            
            // For standard dialogue, use a smaller panel without the image
            //use the pop up panel
         
        }

        dialogueText.text = line.dialogueText;

        // Play voice clip if available
        if (line.voiceLine != null)
        {
            AudioSource.PlayClipAtPoint(line.voiceLine, Camera.main.transform.position);
        }

        // If there are choices, display them; otherwise hide the choice panel
        if (line.choices != null && line.choices.Count > 0)
        {
            DisplayChoices(line.choices);
        }
        else
        {
            HideChoices();
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

    /// <summary>
    /// Called when a dialogue choice is selected.
    /// </summary>
    private void OnChoiceSelected(DialogueChoice selectedChoice)
    {
        HideChoices();

        if (selectedChoice.nextDialogue != null)
        {
            // Continue with the next dialogue line if one is assigned
            DisplayDialogueLine(selectedChoice.nextDialogue);
        }
    }
}
