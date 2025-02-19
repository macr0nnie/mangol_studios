using UnityEngine;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
   //need typing effect for dialogue
    //need to add a delay between each letter 

    public string text;
    //method that starts the typing effect
    public void StartTypingEffect(string text)
    {
        //start typing effect
        StartCoroutine(TypeText(text)); 
    }
    //method that types out the text    
    private IEnumerator TypeText(string text)
    {
        //loop through each letter in the text
        for (int i = 0; i < text.Length; i++)
        {
            //display the text one letter at a time
            Debug.Log(text[i]);
            //wait for a short period of time before displaying the next letter
            yield return new WaitForSeconds(0.1f);
        }
    }

    //stop typing effect if the text is skipped
    public void StopTypingEffect()
    {
        //stop typing effect
        //the string should be displayed all at once
        
        StopAllCoroutines();
    }
}
