using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static Dictionary<GameEvent, Delegate> eventDictionary = new();

    public static void Subscribe<T>(GameEvent eventType, Action<T> listener)
    {
        if (!eventDictionary.ContainsKey(eventType))
            eventDictionary[eventType] = null;

        eventDictionary[eventType] = (Action<T>)eventDictionary[eventType] + listener;
    }

    public static void Unsubscribe<T>(GameEvent eventType, Action<T> listener)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = (Action<T>)eventDictionary[eventType] - listener;
            if (eventDictionary[eventType] == null)
                eventDictionary.Remove(eventType);
        }
    }

    public static void Trigger<T>(GameEvent eventType, T eventData, GameEventSO gameEventSO = null)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate thisEvent))
        {
            Action<T> action = thisEvent as Action<T>;
            action?.Invoke(eventData);
        }

        // If there's a Unity Scriptable Object Event linked, trigger it too
        gameEventSO?.RaiseEvent();
    }
}

public enum GameEvent
{
    ExampleEvent,
    PlayerDeath,
    PlayerRespawn,
    LevelComplete,
}


[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Events/Event")]
public class GameEventSO : ScriptableObject
{
    public UnityEvent onEventRaised;

    public void RaiseEvent()
    {
        onEventRaised?.Invoke();
    }
}