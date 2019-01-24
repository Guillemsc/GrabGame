using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private void Awake()
    {
        InitEvents();
        InitPlayer();
    }

    private void InitEvents()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }

    private void InitPlayer()
    {
        player_movement = gameObject.GetComponent<PlayerMovement>();
        player_animation_controller = gameObject.GetComponentInChildren<PlayerAnimationController>();
    }

    private void OnEvent(Event ev)
    {
        switch(ev.Type())
        {
            case EventType.EVENT_PLAYER_DIES:
                {
                    SetDead(true);

                    player_animation_controller.StrongCameraShake(0.2f);

                    break;
                }

            case EventType.EVENT_LEVEL_LOAD:
                {
                    LevelConnection connection = ev.level_load.spawning_connection;

                    player_animation_controller.SetCameraBounds(ev.level_load.to_load.GetCameraBounds());
                    TeleportPlayer(connection.GetSpawnPos());

                    break;
                }
            case EventType.EVENT_PLAYER_REESPAWNS:
                {
                    LevelConnection connection = LevelManager.Instance.GetLastConnectionSpawned();

                    if(connection != null)
                        TeleportPlayer(connection.GetSpawnPos());

                    SetDead(false);

                    break;
                }
        }
    }

    public void TeleportPlayer(Vector3 pos)
    {
        player_movement.ResetRigidBodyVelocity();

        gameObject.transform.position = pos;

        player_animation_controller.FocusCameraOnPlayer();
    }

    public bool GetRespawning()
    {
        return respawning;
    }

    public void SetDead(bool set)
    {
        dead = set;
    }

    public bool GetDead()
    {
        return dead;
    }

    public void SetChangingLevel(bool set)
    {
        changhing_level = set;
    }

    public bool GetChangingLevel()
    {
        return changhing_level;
    }

    private PlayerMovement player_movement = null;
    private PlayerAnimationController player_animation_controller = null;

    private bool dead = false;
    private bool respawning = false;
    private bool changhing_level = false;

    private bool has_weapon = false;
}
