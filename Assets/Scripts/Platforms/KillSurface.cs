using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSurface : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EntityDetector detector = collision.gameObject.GetComponent<EntityDetector>();

        if (detector != null)
        {
            if (detector.GetEntityType() == EntityType.PLAYER)
            {
                GameObject base_go = detector.GetBaseGo();

                PlayerStats stats = base_go.GetComponent<PlayerStats>();

                if (stats != null)
                {
                    stats.SetDead(true);
                }
            }
        }
    }


}
