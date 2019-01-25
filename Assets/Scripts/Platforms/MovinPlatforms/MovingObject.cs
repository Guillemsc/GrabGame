using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private void Awake()
    {
        InitPoints();
        InitQueue();
        InitMovement();
    }

    private void Update()
    {
        DrawDebug();

        UpdateMovement();
    }

    private void DrawDebug()
    {
        for(int i = 0; i < points.Count; ++i)
        {
            if(i + 1 < points.Count)
            {
                Debug.DrawLine(points[i], points[i + 1], Color.magenta);
            }
            else
            {
                if (loop_circuit)
                {
                    Debug.DrawLine(points[i], points[0], Color.magenta);
                }
            }
        }
    }

    private void InitPoints()
    {
        if(points_parent != null)
        {
            int childs_count = points_parent.transform.childCount;

            for(int i = 0; i < childs_count; ++i)
            {
                Transform curr_child = points_parent.transform.GetChild(i);

                points.Add(curr_child.position);
            }
        }
    }

    private void InitQueue()
    {
        queue_context = QueueEventManager.Instance.CreateContext();
    }

    private void InitMovement()
    {
        if(points.Count > 0)
        {
            gameObject.transform.position = points[0];

            curr_point_index = 1;
        }
    }

    private void UpdateMovement()
    {
        if (!moving)
        {
            if (curr_point_index < points.Count && curr_point_index >= 0)
            {
                Vector3 curr_point = points[curr_point_index];

                queue_context.PushEvent(new 
                    QueueEventPosition(this.gameObject, this.transform.position, curr_point, movement_time, movement_curve));

                queue_context.PushEvent(new QueueEventWaitTime(wait_time_between_point));

                queue_context.LastPushedEventOnFinish(NoMovementFinished);

                moving = true;
            }
            else
            {
                if (!only_one_time)
                {
                    if (loop_circuit)
                    {
                        curr_point_index = 0;
                    }
                    else if (!going_back)
                    {
                        curr_point_index -= 2;
                        going_back = true;
                    }
                    else if (going_back)
                    {
                        curr_point_index += 2;
                        going_back = false;
                    }

                }
            }
        }
    }

    private void NoMovementFinished(QueueEvent ev)
    {
        moving = false;

        if (going_back)
            --curr_point_index;
        else
            ++curr_point_index;
    }

    [SerializeField]
    private bool loop_circuit = false;

    [SerializeField]
    private bool only_one_time = false;

    [SerializeField]
    private float movement_time = 0.0f;

    [SerializeField]
    private float wait_time_between_point = 0.0f;

    [SerializeField]
    private EasingFunctionsType movement_curve = new EasingFunctionsType();

    [SerializeField]
    private GameObject points_parent = null;

    private List<Vector3> points = new List<Vector3>();
    private int curr_point_index = 0;
    private bool moving = false;
    private bool going_back = false;

    private QueueEventContext queue_context = null;
}
