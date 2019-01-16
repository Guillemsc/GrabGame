using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator2D>();
        player_movement = gameObject.GetComponentInParent<PlayerMovement>();
        player_sensors = gameObject.GetComponentInParent<PlayerSensors>();

        CameraManager.Instance.CameraFollow(CameraManager.Instance.GetUsedCamera(), gameObject, 0.2f, new Vector3(0, 1));
    }

    private void Update()
    {
        SetPlayerAnimation();
    }

    private void AnimationIdle()
    {
        if(animator != null)
        {
            animator.PlayAnimation("idle", 0.1f);
        }
    }

    private void AnimationRun()
    {
        if (animator != null)
        {
            animator.PlayAnimation("run", 0.06f);
        }
    }

    private void AnimationWall()
    {
        if (animator != null)
        {
            animator.PlayAnimation("wall_grab", 0.1f);
        }
    }

    private void LookLeft()
    {
        if (animator != null)
        {
            animator.SetFilpX(true);
        }
    }

    private void LookRight()
    {
        if (animator != null)
        {
            animator.SetFilpX(false);
        }
    }

    private void SetPlayerAnimation()
    {
        Vector2Int input = player_movement.GetInput();
        Vector2 rigid_body_vel = player_movement.GetRigidBodyVelocity();
        bool hitting_platform = player_sensors.GetTouchingPlatform();
        bool touching_wall_left = player_sensors.GetTouchingWallLeft();
        bool touching_wall_right = player_sensors.GetTouchingWallRight();

        if (input.x > 0)
        {
            LookRight();
        }
        else if (input.x < 0)
        {
            LookLeft();
        }

        if (rigid_body_vel.x != 0)
            AnimationRun();
        else
            AnimationIdle();

        if (!hitting_platform)
            AnimationRun();

        if (!hitting_platform)
        {
            if (touching_wall_left)
            {
                LookRight();
                AnimationWall();
            }
            else if (touching_wall_right)
            {
                LookLeft();
                AnimationWall();
            }
        }
    }

    private Animator2D animator = null;
    private PlayerMovement player_movement = null;
    private PlayerSensors player_sensors = null;
}
