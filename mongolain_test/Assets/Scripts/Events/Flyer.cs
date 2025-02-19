using UnityEngine;
using UnityEngine.UI;

public class Flyer : MonoBehaviour
{
   //this script has some text that should change when there is a new flyer
    [SerializeField] private TMPro.TextMeshProUGUI flyerText;
    //subscribe to the event
    private void OnEnable()
    {
        EventManager.Subscribe<string>(GameEvent.Quest, OnNewQuest);
    }
    
     // Unsubscribe from the event
    private void OnDisable()
    {
        EventManager.Unsubscribe<string>(GameEvent.Quest, OnNewQuest);
    }

    // Event handler to update the flyer text
    private void OnNewQuest(string questName)
    {
        flyerText.text = "New Quest: " + questName;
    }

}
