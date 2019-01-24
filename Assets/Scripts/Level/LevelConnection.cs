using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelConnection : MonoBehaviour
{
    private void Update()
    {
        if (connection != null)
            Debug.DrawLine(gameObject.transform.position, connection.gameObject.transform.position, Color.yellow);
    }

    public LevelConnection GetConnection()
    {
        return connection;
    }

    public Level GetLevelToLoad()
    {
        return to_load;
    }

    public Level GetLevelToUnload()
    {
        return to_unload;
    }

    public Vector3 GetSpawnPos()
    {
        Vector3 ret = Vector3.zero;

        if (spawn_pos != null)
            ret = spawn_pos.transform.position;

        return ret;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Application.isPlaying)
        {
            if (connection != null && to_unload != null)
            {
                EntityDetector detector = collision.gameObject.GetComponent<EntityDetector>();

                if (detector != null)
                {
                    if (detector.GetEntityType() == EntityType.PLAYER)
                    {
                        LevelManager.Instance.SwapLevels(this);
                    }
                }
            }
        }
    }

    [SerializeField]
    private LevelConnection connection = null;

    [SerializeField]
    private Level to_load = null;

    [SerializeField]
    private Level to_unload = null;

    [SerializeField]
    private GameObject spawn_pos = null;
}
