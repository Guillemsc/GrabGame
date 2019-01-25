using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParenting : MonoBehaviour
{
    private void Awake()
    {
        InitEvents();
    }

    private void InitEvents()
    {
        EventManager.Instance.Suscribe(EventType.EVENT_PLAYER_START_TOUCHING_PLATFORM, OnEvent);
        EventManager.Instance.Suscribe(EventType.EVENT_PLAYER_STOP_TOUCHIING_PLATFORM, OnEvent);
        EventManager.Instance.Suscribe(EventType.EVENT_PLAYER_START_TOUCHING_WALL, OnEvent);
        EventManager.Instance.Suscribe(EventType.EVENT_PLAYER_STOP_TOUCHING_WALL, OnEvent);
    }

    private void OnEvent(Event ev)
    {
        switch(ev.Type())
        {
            case EventType.EVENT_PLAYER_START_TOUCHING_PLATFORM:
                {
                    break;
                }
            case EventType.EVENT_PLAYER_STOP_TOUCHIING_PLATFORM:
                {
                    break;
                }
            case EventType.EVENT_PLAYER_START_TOUCHING_WALL:
                {
                    break;
                }
            case EventType.EVENT_PLAYER_STOP_TOUCHING_WALL:
                {
                    break;
                }
        }
    }
}
