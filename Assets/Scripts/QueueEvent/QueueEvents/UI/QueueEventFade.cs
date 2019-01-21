using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueEventFade : QueueEvent
{
    public QueueEventFade(GameObject element, float starting_val, float ending_val, float fade_time, 
        EasingFunctionsType movement_type, bool real_time = false) : base("fade")
    {
        this.movement_type = movement_type;
        this.affected_element = element;
        this.starting_val = starting_val;
        this.ending_val = ending_val;
        this.real_time = real_time;
        this.fade_time = fade_time;

        val_difference = ending_val - starting_val;
    }

    public override void OnStart()
    {
        canvas_group = affected_element.GetComponent<CanvasGroup>();

        if(canvas_group == null)
            canvas_group = affected_element.AddComponent<CanvasGroup>();

        canvas_group.alpha = starting_val;

        fade_timer.Start();
    }

    public override void OnUpdate()
    {
        float alpha = 0;

        float time = 0.0f;

        if (real_time)
            time = fade_timer.ReadRealTime();
        else
            time = fade_timer.ReadTime();

        alpha = EasingFunctions.GetEasing(movement_type, val_difference, time, starting_val, fade_time);

        canvas_group.alpha = alpha;

        if (time >= fade_time)
        {
            canvas_group.alpha = ending_val;

            Finish();
        }
    }

    private float starting_val = 0.0f;
    private float ending_val = 0.0f;
    private float val_difference = 0.0f;
    private float fade_time = 0.0f;
    private Timer fade_timer = new Timer();
    private GameObject affected_element = null;
    private CanvasGroup canvas_group = null;
    private EasingFunctionsType movement_type = new EasingFunctionsType();
    private bool real_time = false;
}
