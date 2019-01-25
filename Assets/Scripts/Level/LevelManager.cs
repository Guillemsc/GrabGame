using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    LevelManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        InitQueueContext();
    }

    private void Start()
    {
        StartLevel(starting_level);
        LoadLevel(starting_level, starting_level.GetStartConnection());
    }

    private void InitQueueContext()
    {
        queue_context = QueueEventManager.Instance.CreateContext();
    }

    public void StartLevel(Level level)
    {
        if(level != null)
        {
            curr_level = level;

            Event ev = new Event(EventType.EVENT_LEVEL_START);
            ev.level_start.starting_level = level;
            EventManager.Instance.SendEvent(ev);
        }
    }

    public void EndLevel(Level level)
    {
        if (level != null)
        {
            curr_level = null;

            Event ev = new Event(EventType.EVENT_LEVEL_END);
            ev.level_end.ending_level = level;
            EventManager.Instance.SendEvent(ev);
        }
    }

    public void LoadLevel(Level level, LevelConnection used_connection)
    {
        if (level != null)
        {
            Event ev = new Event(EventType.EVENT_LEVEL_LOAD);
            ev.level_load.to_load = level;
            ev.level_load.spawning_connection = used_connection;
            EventManager.Instance.SendEvent(ev);

            last_conneciton_spawned = used_connection;

            level.gameObject.SetActive(true);
        }
    }

    public void UnLoadLevel(Level level)
    {
        if (level != null)
        {
            Event ev = new Event(EventType.EVENT_LEVEL_UNLOAD);
            ev.level_unload.to_unload = level;
            EventManager.Instance.SendEvent(ev);

            level.gameObject.SetActive(false);
        }
    }

    public void SwapLevels(LevelConnection level_connection)
    {        
        if (level_connection != null)
        {
            if (!swaping_level)
            {
                swaping_level = true;

                curr_level_connection_used = level_connection;

                LevelConnection connection_with = curr_level_connection_used.GetConnection();

                EndLevel(curr_level_connection_used.GetLevelToUnload());

                StartLevelSwapAnimation();
            }
        }
        
    }

    public Level GetCurrLevel()
    {
        return curr_level;
    }

    public LevelConnection GetLastConnectionSpawned()
    {
        return last_conneciton_spawned;
    }

    private void StartLevelSwapAnimation()
    {
        if (swaping_level)
        {
            queue_context.PushEvent(new 
                QueueEventFade(level_swaping_image.gameObject, 0, 0, 0.0f, EasingFunctionsType.QUAD_OUT));

            queue_context.PushEvent(new QueueEventSetActive(level_swaping_image.gameObject, true));
            queue_context.PushEvent(new 
                QueueEventFade(level_swaping_image.gameObject, 0, 1, levels_swap_time, EasingFunctionsType.QUAD_IN));
            queue_context.LastPushedEventOnFinish(OnLevelsNeedSwap);

            queue_context.PushEvent(new 
                QueueEventFade(level_swaping_image.gameObject, 1, 0, levels_swap_time, EasingFunctionsType.QUAD_OUT));
            queue_context.PushEvent(new 
                QueueEventSetActive(level_swaping_image.gameObject, false));
            queue_context.LastPushedEventOnFinish(OnLevelsSwapChange);
        }
    }

    private void OnLevelsNeedSwap(QueueEvent ev)
    {
        if(curr_level_connection_used != null)
        {
            Level to_load = curr_level_connection_used.GetLevelToLoad();
            Level to_unload = curr_level_connection_used.GetLevelToUnload();

            UnLoadLevel(to_unload);
            LoadLevel(to_load, curr_level_connection_used.GetConnection());
        }
    }

    private void OnLevelsSwapChange(QueueEvent ev)
    {
        swaping_level = false;

        if (curr_level_connection_used)
        {
            Level to_load = curr_level_connection_used.GetLevelToLoad();

            StartLevel(to_load);
        }
    }

    [SerializeField]
    private Level starting_level = null;

    [SerializeField]
    private Image level_swaping_image = null;

    [SerializeField]
    private float levels_swap_time = 0.0f;

    private Level curr_level = null;

    private bool swaping_level = false;
    private LevelConnection curr_level_connection_used = null;

    private LevelConnection last_conneciton_spawned = null;

    private QueueEventContext queue_context = null;
}
