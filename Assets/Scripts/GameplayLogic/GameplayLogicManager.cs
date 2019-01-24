using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLogicManager : Singleton<GameplayLogicManager>
{
    GameplayLogicManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        InitEvents();
    }

    private void Update()
    {
        CheckPlayerReespawn();
    }

    private void InitEvents()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }

    private void OnEvent(Event ev)
    {
        switch(ev.Type())
        {
            case EventType.EVENT_PLAYER_DIES:
                {
                    dead_player = ev.player_dies.player;

                    player_dead_timer.Start();

                    break;
                }
        }
    }

    private void CheckPlayerReespawn()
    {
        if (dead_player != null)
        {
            if (player_dead_timer.ReadTime() > player_dead_time)
            {
                Event ev = new Event(EventType.EVENT_PLAYER_REESPAWNS);
                ev.player_reespawns.player = dead_player;
                EventManager.Instance.SendEvent(ev);

                dead_player = null;
            }
        }
    }

    [SerializeField]
    private float player_dead_time = 0.0f;

    private Timer player_dead_timer = new Timer();
    private GameObject dead_player = null;
}
