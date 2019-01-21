using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    PLAYER,

}

public class EntityDetector : MonoBehaviour
{
    public void OnEnable()
    {
        if(EntityManager.Instance != null)
            EntityManager.Instance.AddEntity(this);
    }

    public void OnDisable()
    {
        if (EntityManager.Instance != null)
            EntityManager.Instance.RemoveEntity(this);
    }

    public void OnDestroy()
    {
        if(EntityManager.Instance != null)
            EntityManager.Instance.RemoveEntity(this);
    }

    public EntityType GetEntityType()
    {
        return type;
    }

    public GameObject GetBaseGo()
    {
        return entity_base_go;
    }

    [SerializeField]
    private EntityType type = new EntityType();

    [SerializeField]
    private GameObject entity_base_go = null;
}
