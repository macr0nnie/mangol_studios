using UnityEngine;

public class TestingEvents : MonoBehaviour
{
    void OnEnable()
{
    EventManager.Subscribe<int>(GameEvent.PlayerDeath, OnPlayerDeath);
}

void OnDisable()
{
    EventManager.Unsubscribe<int>(GameEvent.PlayerDeath, OnPlayerDeath);
}

void OnPlayerDeath(int score)
{
    Debug.Log($"Player died with score: {score}");
}
}
