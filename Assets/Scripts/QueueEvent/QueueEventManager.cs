using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueEventManager : Singleton<QueueEventManager>
{
    QueueEventManager()
    {
        InitInstance(this);
    }

    private void Update()
    {
        UpdateEvents();
    }

    public void PushEvent(QueueEvent ev, bool start_with_last = false)
    {
        if(ev != null)
        {
            if(start_with_last)
                ev.PushWithLastEvent();

            events_to_push.Add(ev);
        }
    }

    public void LastPushedEventOnStart(DelQueueEvent callback)
    {
        if (events_to_push.Count > 0)
        {
            events_to_push[events_to_push.Count].SuscribeToOnEventStart(callback);
        }
    }

    public void LastPushedEventOnFinish(DelQueueEvent callback)
    {
        if(events_to_push.Count > 0)
        {
            events_to_push[events_to_push.Count].SuscribeToOnEventFinish(callback);
        }
    }

    public void UpdateEvents()
    {
        if(curr_events.Count > 0)
        {
            for(int i = 0; i < curr_events.Count;)
            {
                QueueEvent curr_event = curr_events[i];

                if(!curr_event.GetFinished())
                {
                    curr_event.OnUpdate();

                    ++i;
                }
                else
                {
                    curr_event.OnFinish();

                    curr_event.CallOnEventFinish();

                    curr_events.RemoveAt(i);
                }
            }
        }
        else
        {
            if(events_to_push.Count > 0)
            {
                for(int i = 0; i < events_to_push.Count;)
                {
                    QueueEvent curr_event = curr_events[i];

                    bool add = false;

                    if (i == 0)
                    {
                        add = true;
                    }
                    else if (curr_event.GetPushWithLastEvent())
                    {
                        add = true;
                    }

                    if (add)
                    {
                        curr_events.Add(curr_event);

                        events_to_push.RemoveAt(i);

                        curr_event.CallOnEventStart();
                    }
                    else
                        break;
                }
            }
        }
    }

    List<QueueEvent> events_to_push = new List<QueueEvent>();
    List<QueueEvent> curr_events = new List<QueueEvent>();
}
