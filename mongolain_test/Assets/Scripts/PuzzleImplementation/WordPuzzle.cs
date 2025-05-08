using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPuzzle : PuzzleBase
{
    //the text input for the secret code// word that the player has to guess
    private string wordToGuess;
    private string currentGuess;

    //we use override to add extra functionality to the exisitng base class
    public override void StartPuzzle(string word)
    {
        wordToGuess = word;
        //start the puzzle
        base.StartPuzzle();
    }
    public override void Reset()
    {
        base.Reset();
        //reset the word to guess
        currentGuess = string.Empty;
    }
    public override void CompletePuzzle()
    {
        base.CompletePuzzle();
        //check if the word is correct
        if (currentGuess == wordToGuess)
        {
            //call the event, this is what the interface we added was for
            onPuzzleComplete?.Invoke(this); 
        }
    }
}