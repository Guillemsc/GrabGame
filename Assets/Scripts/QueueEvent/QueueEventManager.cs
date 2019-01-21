using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueEventManager : Singleton<QueueEventManager>
{
    QueueEventManager()
    {
        InitInstance(this);
    }

    private void LateUpdate()
    {
        UpdateContexts();
    }

    public QueueEventContext CreateContext()
    {
        QueueEventContext qe = new QueueEventContext();

        contexts.Add(qe);

        return qe;
    }

    public void RemoveContext(QueueEventContext context)
    {
        if (context != null)
            contexts.Remove(context);
    }

    private void UpdateContexts()
    {
        for(int i = 0; i < contexts.Count; ++i)
            contexts[i].UpdateEvents();
    }

    private List<QueueEventContext> contexts = new List<QueueEventContext>();
}
