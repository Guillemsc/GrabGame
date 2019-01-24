using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    EventManager()
    {
        InitInstance(this);
    }

    public void SendEvent(Event ev)
    {
        if (ev != null)
        {
            if (OnEvent != null)
                OnEvent(ev);
        }
    }

    public void Suscribe(OnEventDel del)
    {
        bool found = false;

        if (OnEvent != null)
        {
            foreach (Delegate d in OnEvent.GetInvocationList())
            {
                if ((OnEventDel)d == del)
                {
                    found = true;
                    break;
                }
            }
        }

        if (!found)
            OnEvent += del;
    }

    public void UnSuscribe(OnEventDel del)
    {
        OnEvent -= del;
    }

    public delegate void OnEventDel(Event ev);
    private event OnEventDel OnEvent = null;
}