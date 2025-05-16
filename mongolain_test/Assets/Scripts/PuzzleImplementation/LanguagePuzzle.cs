using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class LanguagePuzzle : PuzzleBase
{
    // Text to display encrypted
    public string encryptedText;
    public List<string> discoveredWords = new List<string>();
    public List<string> totalWords = new List<string>();

    // Text UI component to display the puzzle text
    public TMPro.TextMeshProUGUI puzzleTextDisplay;

    public void StartPuzzle(string text)
    {
        encryptedText = text;
        discoveredWords.Clear();
        //  UpdateWordsDisplayed();
        // Start the puzzle
        base.StartPuzzle();
    }

    public override void Reset()
    {
        discoveredWords.Clear();
        // UpdateWordsDisplayed();
    }

    public void UnlockWord(string newlyDiscoveredWord)
    {
        // Check if the word is in the list and not already discovered
        if (totalWords.Contains(newlyDiscoveredWord) && !discoveredWords.Contains(newlyDiscoveredWord))
        {
            // Add it to the discovered words
            discoveredWords.Add(newlyDiscoveredWord);

            //UpdateWordsDisplayed();

            // Check if all words are discovered
            if (discoveredWords.Count == totalWords.Count)
            {
                CompletePuzzle();
            }
        }
    }
}

    /*
    {
        string displayText = encryptedText;

        // Replace placeholder tokens with discovered words
        foreach (string word in discoveredWords)
        {
            // Assume format like "[WORD1]" as placeholder in encrypted text
            string placeholder = $"[{totalWords.IndexOf(word) + 1}]";
            displayText = displayText.Replace(placeholder, word);
        }

        // Update the UI
        if (puzzleTextDisplay != null)
        {
            puzzleTextDisplay.text = displayText;
        }
        else
        {
            Debug.LogWarning("No text display assigned to LanguagePuzzle.");
        }
    }
    
    public void UpdateWordsDisplayed()
    {
        if (puzzleTextDisplay == null) return;

        // Set base font to ancient
        puzzleTextDisplay.font = ancientFont;

        string displayText = encryptedText;

        // Use rich text to apply normal font to discovered words
        foreach (string word in discoveredWords)
        {
            // Find and wrap each instance of this word with font tag
            string pattern = $"\\b{word}\\b";
            string replacement = $"<font=\"{normalFont.name}\">{word}</font>";

            // Use regex to only replace whole words
            displayText = System.Text.RegularExpressions.Regex.Replace(
                displayText,
                pattern,
                replacement,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        puzzleTextDisplay.text = displayText;
    }
    */

