using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DelLevelSwap();

public class LevelSwaperManager : Singleton<LevelSwaperManager>
{
    LevelSwaperManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        queue_context = QueueEventManager.Instance.CreateContext();
    }

    public void StartLevelSwapAnimation(float fade_time, DelLevelSwap to_change_level_callback)
    {
        if(!swaping_level)
        {
            swaping_level = true;

            on_to_change_level = null;

            on_to_change_level += to_change_level_callback;

            queue_context.PushEvent(new QueueEventFade(level_swaper_image.gameObject, 0, 0, 0.0f, EasingFunctionsType.QUAD_OUT));
            queue_context.PushEvent(new QueueEventSetActive(level_swaper_image.gameObject, true));
            queue_context.PushEvent(new QueueEventFade(level_swaper_image.gameObject, 0, 1, fade_time, EasingFunctionsType.QUAD_IN));
            queue_context.LastPushedEventOnFinish(ToChangeLevel);
            queue_context.PushEvent(new QueueEventFade(level_swaper_image.gameObject, 1, 0, fade_time, EasingFunctionsType.QUAD_OUT));
            queue_context.PushEvent(new QueueEventSetActive(level_swaper_image.gameObject, false));
            queue_context.LastPushedEventOnFinish(LevelChangeFinished);
        }
    }

    private void ToChangeLevel(QueueEvent ev)
    {
        if (on_to_change_level != null)
            on_to_change_level();
    }

    private void LevelChangeFinished(QueueEvent ev)
    {
        swaping_level = false;
    }

    [SerializeField]
    Image level_swaper_image = null;

    [SerializeField]
    private GameObject starting_level = null;

    private DelLevelSwap on_to_change_level = null;
    private bool swaping_level = false;

    private QueueEventContext queue_context = null;
}
