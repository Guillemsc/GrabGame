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
                        if (!player_spawning)
                        {
                            GameObject go_to_change_pos = detector.GetBaseGo();

                            player_to_change_pos = go_to_change_pos.GetComponent<PlayerStats>();

                            if (player_to_change_pos != null)
                                player_to_change_pos.SetChangingLevel(true);

                            connection.PlayerSpawning();

                            LevelConnectionManager.Instance.StartLevelSwapAnimation(OnLevelSwap, OnLevelSwapFinish);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EntityDetector detector = collision.gameObject.GetComponent<EntityDetector>();

        if (detector != null)
        {
            if (detector.GetEntityType() == EntityType.PLAYER)
            {
                if (player_spawning)
                {
                    player_spawning = false;
                }
            }
        }
    }

    public void PlayerSpawning()
    {
        player_spawning = true;
    }

    private void OnLevelSwap()
    {
        if (to_unload != null)
            to_unload.gameObject.SetActive(false);

        if (to_load != null)
            to_load.gameObject.SetActive(true);

        player_to_change_pos.transform.position = connection.gameObject.transform.position;
    }

    private void OnLevelSwapFinish()
    {
        player_to_change_pos.SetChangingLevel(false);
    }

    [SerializeField]
    private LevelConnection connection = null;

    [SerializeField]
    private GameObject to_load = null;

    [SerializeField]
    private GameObject to_unload = null;

    private PlayerStats player_to_change_pos = null;

    private bool player_spawning = false;
}
