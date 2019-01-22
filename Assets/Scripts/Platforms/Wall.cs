using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool GetCanWallJump()
    {
        return can_wall_jump;
    }

    [SerializeField]
    private bool can_wall_jump = true;
}
