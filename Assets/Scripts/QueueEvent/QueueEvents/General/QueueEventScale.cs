using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueEventScale : QueueEvent
{
    public QueueEventScale(GameObject element, Vector3 starting_val, Vector3 ending_val, float scale_time,
        EasingFunctionsType movement_type, bool real_time = false) : base("scale")
    {
        this.movement_type = movement_type;
        this.affected_element = element;
        this.starting_val = starting_val;
        this.ending_val = ending_val;
        this.real_time = real_time;
        this.scale_time = scale_time;

        val_difference = ending_val - starting_val;
    }

    public override void OnStart()
    {
        affected_element.transform.localScale = starting_val;

        scale_timer.Start();
    }

    public override void OnUpdate()
    {
        float val_x = 0.0f;
        float val_y = 0.0f;
        float val_z = 0.0f;

        float time = 0.0f;

        if (real_time)
            time = scale_timer.ReadRealTime();
        else
            time = scale_timer.ReadTime();

        val_x = EasingFunctions.GetEasing(movement_type, val_difference.x, time, starting_val.x, scale_time);
        val_y = EasingFunctions.GetEasing(movement_type, val_difference.y, time, starting_val.y, scale_time);
        val_z = EasingFunctions.GetEasing(movement_type, val_difference.z, time, starting_val.z, scale_time);

        affected_element.transform.localScale = new Vector3(val_x, val_y, val_z);

        if (time >= scale_time)
        {
            affected_element.transform.localScale = ending_val;

            Finish();
        }
    }

    private Vector3 starting_val = Vector3.zero;
    private Vector3 ending_val = Vector3.zero;
    private Vector3 val_difference = Vector3.zero;
    private float scale_time = 0.0f;
    private Timer scale_timer = new Timer();
    private GameObject affected_element = null;
    private EasingFunctionsType movement_type = new EasingFunctionsType();
    private bool real_time = false;
}