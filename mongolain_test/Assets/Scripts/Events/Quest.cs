using UnityEngine;
using System.Collections.Generic;

public class Quest : MonoBehaviour
{
    private List<string> quests = new List<string>
    {
        "Find the treasure!",
        "Defeat the dragon!",
        "Rescue the princess!",
        "Collect 10 herbs!"
    };

    private int currentQuestIndex = 0;

    private void Start()
    {
        TriggerCurrentQuest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Every time the space is pressed, the quest changes to the next one
            currentQuestIndex = (currentQuestIndex + 1) % quests.Count;
            TriggerCurrentQuest();
        }
    }

    private void TriggerCurrentQuest()
    {
        EventManager.Trigger<string>(GameEvent.Quest, quests[currentQuestIndex]);
    }
}