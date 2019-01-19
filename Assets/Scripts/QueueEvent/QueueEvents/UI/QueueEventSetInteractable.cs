using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueEventSetInteractable : QueueEvent
{
    public QueueEventSetInteractable(GameObject element, bool set_interactable) : base("set_interactable")
    {
        this.affected_element = element;
        this.set_interactable = set_interactable;
    }

    public override void OnStart()
    {
        canvas_group = affected_element.GetComponent<CanvasGroup>();

        if (canvas_group == null)
            canvas_group = affected_element.AddComponent<CanvasGroup>();

        canvas_group.interactable = set_interactable;

        Finish();
    }

    private GameObject affected_element = null;
    private CanvasGroup canvas_group = null;
    private bool set_interactable = false;
}
