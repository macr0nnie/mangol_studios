using UnityEngine;

public class Celebrations : MonoBehaviour
{
    public GameEventSO celebrationEvent;

    private void OnEnable()
    {
        celebrationEvent.onEventRaised.AddListener(OnCelebrationEvent);
    }

    private void OnDisable()
    {
        celebrationEvent.onEventRaised.RemoveListener(OnCelebrationEvent);
    }

    private void OnCelebrationEvent()
    {
        Debug.Log("Celebration event triggered!");
    }
}
