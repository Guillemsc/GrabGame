using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwaper : MonoBehaviour
{
    private void Awake()
    {
        if (detector != null)
            collision_detector = detector.GetComponent<CollisionDetector>();

        if(collision_detector != null)
            collision_detector.SuscribeOnTriggerEnter2D(DetectorOnTriggerEnter2D);
    }

    private void Update()
    {
        if (detector != null && position_on_load_level != null)
            Debug.DrawLine(detector.transform.position, position_on_load_level.transform.position, Color.yellow);
    }

    private void DetectorOnTriggerEnter2D(Collider2D coll)
    {
        EntityDetector detector = coll.gameObject.GetComponent<EntityDetector>();

        if(detector != null)
        {
            if(detector.GetEntityType() == EntityType.PLAYER)
            {
                go_to_swap = detector.GetBaseGo();

                LevelSwaperManager.Instance.StartLevelSwapAnimation(fade_time, OnLevelSwap);
            }
        }
    }

    private void OnLevelSwap()
    {
        if (to_unload != null)
            to_unload.gameObject.SetActive(false);

        if (to_load != null)
            to_load.gameObject.SetActive(true);

        go_to_swap.transform.position = position_on_load_level.transform.position;
    }

    [SerializeField]
    private float fade_time = 0.0f;

    [SerializeField]
    private GameObject detector = null;

    [SerializeField]
    private GameObject to_unload = null;

    [SerializeField]
    private GameObject to_load = null;

    [SerializeField]
    private GameObject position_on_load_level = null;

    private CollisionDetector collision_detector = null;

    private GameObject go_to_swap = null;
}
