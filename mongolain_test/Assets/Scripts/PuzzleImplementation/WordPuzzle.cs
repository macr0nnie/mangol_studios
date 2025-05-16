using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPuzzle : PuzzleBase
{
    //the text input for the secret code// word that the player has to guess;
    [SerializeField] private string wordToGuess;
    private string currentGuess = string.Empty;

    public override void StartPuzzle()
    {
        base.Start(); // Changed from StartPuzzle() to match the parent
    }

    public override void Reset()
    {
        base.Reset();
        //reset the word to guess
        currentGuess = string.Empty;
    }

    // This method should check the word and then call CompletePuzzle if correct
    public void CheckGuess(string guess)
    {
        currentGuess = guess;
        
        if (currentGuess == wordToGuess)
        {
            // Call the protected CompletePuzzle method from the base class
            base.CompletePuzzle();
        }
    }

    // Remove the CompletePuzzle override since it has incorrect logic
    // The base.CompletePuzzle() already handles the event invocation
}