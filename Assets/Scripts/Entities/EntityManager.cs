using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    EntityManager()
    {
        InitInstance(this);
    }

    public void AddEntity(EntityDetector detector)
    {
        if (detector != null)
        {
            RemoveEntity(detector);

            List<EntityDetector> entities_list = null;

            if (entities.ContainsKey(detector.GetEntityType()))
            {
                entities_list = entities[detector.GetEntityType()];
            }
            else
            {
                entities_list = new List<EntityDetector>();
                entities[detector.GetEntityType()] = entities_list;
            }

            entities_list.Add(detector);
        }
    }

    public void RemoveEntity(EntityDetector detector)
    {
        if(entities.ContainsKey(detector.GetEntityType()))
        {
            List<EntityDetector> entities_list = entities[detector.GetEntityType()];

            entities_list.Remove(detector);
        }
    }

    public List<EntityDetector> GetEntitiesofType(EntityType type)
    {
        List<EntityDetector> ret = null;

        if (entities.ContainsKey(type))
        {
            ret = entities[type];
        }

        return ret;
    }

    Dictionary<EntityType, List<EntityDetector>> entities = new Dictionary<EntityType, List<EntityDetector>>();
}
