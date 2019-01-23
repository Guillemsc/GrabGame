using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DelLevelSwap();

public class LevelConnectionManager : Singleton<LevelConnectionManager>
{
    LevelConnectionManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        queue_context = QueueEventManager.Instance.CreateContext();
    }

    public void StartLevelSwapAnimation(DelLevelSwap to_change_level_callback, DelLevelSwap on_change_level_finished_callback)
    {
        if(!swaping_level)
        {
            swaping_level = true;

            on_to_change_level = null;
            on_change_level_finished = null;

            this.on_to_change_level += to_change_level_callback;
            this.on_change_level_finished += on_change_level_finished_callback;

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

        if (on_change_level_finished != null)
            on_change_level_finished();
    }

    [SerializeField]
    private Image level_swaper_image = null;

    [SerializeField]
    private float fade_time = 0.0f;

    private DelLevelSwap on_to_change_level = null;
    private DelLevelSwap on_change_level_finished = null;
    private bool swaping_level = false;

    private QueueEventContext queue_context = null;
}
