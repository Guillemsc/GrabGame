using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private void Awake()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    public bool GetRespawning()
    {
        return respawning;
    }

    public void SetChangingLevel(bool set)
    {
        changhing_level = set;

        if (movement != null)
        {
            movement.SetMovementEnabled(!changhing_level);

            movement.GetRigidBody().velocity = new Vector2(0, 0);
        }
    }

    public bool GetChangingLevel()
    {
        return changhing_level;
    }

    private PlayerMovement movement = null;

    private bool respawning = false;
    private bool changhing_level = false;

    private bool has_weapon = false;
}
