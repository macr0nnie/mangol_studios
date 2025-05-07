using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue Panel Components")]
    // Panel game objects
    public GameObject cutscene_panel;        // Full screen panel
    public GameObject top_screen_text_panel; // Text at top of screen
    public GameObject bubbles_panel;         // Hovering speech bubbles
    
    // Text references for each panel
    public TMP_Text cutsceneText;
    public TMP_Text bubblesText;
    public TMP_Text top_screen_text; 
    
    // Images
    public Image dialogue_speaker_image;     // Speaker portrait
    public Image full_screen_Image;          // Background image for cutscene
    
    [Header("Typing Effect Settings")]
    [Range(0.01f, 0.5f)]
    public float typingSpeed = 0.05f;
    public float punctuationPause = 0.2f;
    public AudioSource typingSoundSource;
    public float typingSoundFrequency = 2;
    
    [Header("Choice Panel Components")]
    public GameObject choicePanel;           // Panel for choice buttons
    public List<Button> choiceButtons;       // Pre-assigned choice buttons
    
    // Current dialogue state
    private DialogueLine currentDialogueLine;
    private AudioSource currentVoiceLine;    // Reference to currently playing voice
    private TextTypingManager typingManager;
    
    // Start is called before the first frame update
    public void Start()
    {
        // Initialize the typing manager
        typingManager = new TextTypingManager(typingSpeed, punctuationPause, typingSoundSource, typingSoundFrequency);
        
        // Ensure all panels are hidden at start
        HideAllPanels();
    }
    
    // Hide all dialogue panels
    private void HideAllPanels()
    {
        cutscene_panel.SetActive(false);
        bubbles_panel.SetActive(false);
        choicePanel.SetActive(false);
        top_screen_text_panel.SetActive(false);
    }
    
    // Display a dialogue line on the appropriate panel
    public void DisplayDialogueLine(DialogueLine line)
    {
        // Stop any current typing animation
        typingManager.StopAllTyping();
        
        // Store current line and hide all panels first
        currentDialogueLine = line;
        HideAllPanels();
        
        // Stop any currently playing voice line
        if (currentVoiceLine != null)
        {
            Destroy(currentVoiceLine.gameObject);
        }
        
        // Setup the appropriate panel based on display type
        switch (line.displayType)
        {
            case DialogueDisplayType.Cutscene:
                SetupCutscenePanel(line);
                break;
                
            case DialogueDisplayType.Standard:
                SetupBubblesPanel(line);
                break;
                
            case DialogueDisplayType.SpeachBubble:
                SetupTopScreenPanel(line);
                break;
        }
        
        // Play voice clip if available
        if (line.voiceLine != null)
        {
            currentVoiceLine = AudioSource.PlayClipAtPoint(line.voiceLine, Camera.main.transform.position, 1.0f);
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
    
    // Setup the cutscene panel with the dialogue line
    private void SetupCutscenePanel(DialogueLine line)
    {
        cutscene_panel.SetActive(true);
        
        // Clear the text initially
        cutsceneText.text = "";
        
        // Set the background image if available
        if (line.image != null)
        {
            full_screen_Image.sprite = line.image;
            full_screen_Image.gameObject.SetActive(true);
        }
        else
        {
            full_screen_Image.gameObject.SetActive(false);
        }
        
        // Start typing animation
        typingManager.StartTyping(cutsceneText, line.dialogueText);
    }
    
    // Setup the bubbles panel with the dialogue line
    private void SetupBubblesPanel(DialogueLine line)
    {
        bubbles_panel.SetActive(true);
        
        // Clear the text initially
        bubblesText.text = "";
        
        // Set the speaker image if available
        if (line.image != null)
        {
            dialogue_speaker_image.sprite = line.image;
            dialogue_speaker_image.gameObject.SetActive(true);
        }
        else
        {
            dialogue_speaker_image.gameObject.SetActive(false);
        }
        
        // Start typing animation
        typingManager.StartTyping(bubblesText, line.dialogueText);
    }
    
    // Setup the top screen panel with the dialogue line
    private void SetupTopScreenPanel(DialogueLine line)
    {
        top_screen_text_panel.SetActive(true);
        
        // Clear the text initially
        top_screen_text.text = "";
        
        // Start typing animation
        typingManager.StartTyping(top_screen_text, line.dialogueText);
    }
    
    // Display choices for the player to select
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
    
    // Hide all choice buttons
    private void HideChoices()
    {
        choicePanel.SetActive(false);
        foreach (Button btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
    }
    
    // Called when a choice is selected
    private void OnChoiceSelected(DialogueChoice selectedChoice)
    {
        HideChoices();

        if (selectedChoice.nextDialogue != null)
        {
            // Continue with the next dialogue line if one is assigned
            DisplayDialogueLine(selectedChoice.nextDialogue);
        }
    }
    
    // Skip the current dialogue animation (display full text immediately)
    public void SkipDialogueAnimation()
    {
        typingManager.SkipTyping();
    }
}

// Internal class to handle text typing animations
public class TextTypingManager
{
    private float typingSpeed;
    private float punctuationPause;
    private AudioSource soundSource;
    private float soundFrequency;
    
    private Dictionary<TMP_Text, Coroutine> activeTypingCoroutines;
    private Dictionary<TMP_Text, string> fullTextContents;
    private MonoBehaviour coroutineRunner;
    
    public TextTypingManager(float speed, float pauseDuration, AudioSource audio, float frequency)
    {
        typingSpeed = speed;
        punctuationPause = pauseDuration;
        soundSource = audio;
        soundFrequency = frequency;
        
        activeTypingCoroutines = new Dictionary<TMP_Text, Coroutine>();
        fullTextContents = new Dictionary<TMP_Text, string>();
        
        // We need a reference to a MonoBehaviour to run coroutines
        coroutineRunner = Object.FindObjectOfType<DialogueUIController>();
        
        if (coroutineRunner == null)
        {
            Debug.LogError("TextTypingManager requires a DialogueUIController in the scene to run coroutines.");
        }
    }
    
    public void StartTyping(TMP_Text textComponent, string fullText, UnityAction onComplete = null)
    {
        if (textComponent == null)
            return;
            
        // Stop any existing typing on this text component
        StopTyping(textComponent);
        
        // Store the full text
        fullTextContents[textComponent] = fullText;
        
        // Start a new typing coroutine
        activeTypingCoroutines[textComponent] = coroutineRunner.StartCoroutine(TypeText(textComponent, fullText, onComplete));
    }
    
    public void StopTyping(TMP_Text textComponent, bool showFullText = true)
    {
        if (textComponent == null)
            return;
            
        if (activeTypingCoroutines.ContainsKey(textComponent))
        {
            coroutineRunner.StopCoroutine(activeTypingCoroutines[textComponent]);
            activeTypingCoroutines.Remove(textComponent);
            
            if (showFullText && fullTextContents.ContainsKey(textComponent))
            {
                textComponent.text = fullTextContents[textComponent];
            }
        }
    }
    
    public void StopAllTyping(bool showFullText = true)
    {
        foreach (var textComponent in new List<TMP_Text>(activeTypingCoroutines.Keys))
        {
            StopTyping(textComponent, showFullText);
        }
    }
    
    public void SkipTyping()
    {
        StopAllTyping(true);
    }
    
    public bool IsTyping(TMP_Text textComponent)
    {
        return activeTypingCoroutines.ContainsKey(textComponent);
    }
    
    private IEnumerator TypeText(TMP_Text textComponent, string fullText, UnityAction onComplete)
    {
        string currentText = "";
        int charCount = 0;
        
        foreach (char c in fullText)
        {
            currentText += c;
            textComponent.text = currentText;
            charCount++;
            
            // Play typing sound at intervals
            if (soundSource != null && charCount % soundFrequency == 0)
            {
                soundSource.pitch = Random.Range(0.8f, 1.2f);
                soundSource.Play();
            }
            
            // Add pauses for punctuation
            if (c == '.' || c == '!' || c == '?')
            {
                yield return new WaitForSeconds(punctuationPause);
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                yield return new WaitForSeconds(punctuationPause / 2);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        
        // Remove from active coroutines
        if (activeTypingCoroutines.ContainsKey(textComponent))
        {
            activeTypingCoroutines.Remove(textComponent);
        }
        
        onComplete?.Invoke();
    }
}