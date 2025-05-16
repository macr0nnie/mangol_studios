using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue Panel Components")]
    //game cutscne style panels
    public GameObject cutscene_panel;//full cutscene
    public GameObject top_screen_panel;
    public GameObject bubbles_panel;
    //text references for each panel
    public TMP_Text cutsceneText;
    public TMP_Text bubblesText;
    public TMP_Text top_screen_text; //text for the top screen panel
    //images
    public Image dialouge_speaker_image;
    public Image full_screen_Image;          

    [Header("Choice Panel Components")]
    public GameObject choicePanel;             // Panel that contains the choice buttons
    public List<Button> choiceButtons;         // Pre-assigned buttons to display choices
    private DialogueLine currentDialogueLine;  // The current dialogue dialogueLine being shown

    public void Start()
    {
        //should i make on large dialouge panel instead?
        cutscene_panel.SetActive(false);
        bubbles_panel.SetActive(false);
        choicePanel.SetActive(false);
        top_screen_panel.SetActive(false);
    }
    public void DisplayDialogueLine(DialogueLine dialogueLine)
    {
        currentDialogueLine = dialogueLine;
        switch (dialogueLine.displayType)
        {
            case DialogueDisplayType.Standard:
                bubbles_panel.SetActive(true);
                cutscene_panel.SetActive(false);
               
                break;
            case DialogueDisplayType.Cutscene:
                // Set up the cutscene panel
                cutscene_panel.SetActive(true);
                bubbles_panel.SetActive(false);
                cutsceneText.text = dialogueLine.dialogueText;
                break;
            case DialogueDisplayType.SpeachBubble:
                // Set up the speech bubble panel
                top_screen_panel.SetActive(true);
                bubblesText.text = dialogueLine.dialogueText;
                break;
        }
        // If there are choices, display them; otherwise hide the choice panel
        if (dialogueLine.choices != null && dialogueLine.choices.Count > 0)
        {
            DisplayChoices(dialogueLine.choices);
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
    private void OnChoiceSelected(DialogueChoice selectedChoice)
    {
        HideChoices();
        if (selectedChoice.nextDialogue != null)
        {
            DisplayDialogueLine(selectedChoice.nextDialogue); //go to the next assignedline for the dialouge
        }
    }
}
