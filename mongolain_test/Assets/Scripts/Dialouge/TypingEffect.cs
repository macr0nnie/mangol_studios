using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class TypingEffect : MonoBehaviour
{
    [Header("Typing Settings")]
    [Range(0.01f, 0.5f)]
    public float typingSpeed = 0.05f;
    public float punctuationPause = 0.2f;
    
    [Header("Audio")]
    public AudioSource typingSoundSource;
    public float typingSoundFrequency = 2; // Play sound every N characters
    
    [Header("Events")]
    public UnityEvent onTypingComplete;
    
    private TMP_Text textComponent;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    
    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        if (textComponent == null)
        {
            Debug.LogError("TypingEffect requires a TextMeshProUGUI component on the same GameObject.");
        }
    }
    
    public void StartTypingEffect(string fullText)
    {
        if (isTyping)
        {
            StopTypingEffect();
        }
        
        textComponent.text = "";
        typingCoroutine = StartCoroutine(TypeText(fullText));
    }
    
    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        string currentText = "";
        int charCount = 0;
        
        foreach (char c in fullText)
        {
            currentText += c;
            textComponent.text = currentText;
            charCount++;
            
            // Play typing sound at intervals
            if (typingSoundSource != null && charCount % typingSoundFrequency == 0)
            {
                typingSoundSource.pitch = Random.Range(0.8f, 1.2f);
                typingSoundSource.Play();
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
        
        isTyping = false;
        onTypingComplete?.Invoke();
    }
    
    public void StopTypingEffect(bool displayFullText = true)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
            
            if (displayFullText && textComponent != null)
            {
                // This assumes we have a reference to the full text somewhere
                // You might need to modify this based on your implementation
                textComponent.text = textComponent.text;
            }
            onTypingComplete?.Invoke();
        }
    }
    public bool IsTyping()
    {
        return isTyping;
    }
}