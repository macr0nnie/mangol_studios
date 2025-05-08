using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPuzzle : PuzzleBase
{
   private [string] wordToGuess;

    //start the puzzle and set the word to guess
    public void StartPuzzle(string word)
    {
        wordToGuess = word;
        //start the puzzle
        base.StartPuzzle();
    }
}