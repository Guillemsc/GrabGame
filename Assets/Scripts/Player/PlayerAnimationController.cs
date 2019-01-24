using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private void Awake()
    {
        InitPlayer();

        camera_follow_item = CameraManager.Instance.
            CameraFollow(CameraManager.Instance.GetUsedCamera(), gameObject, 0.2f, new Vector3(0, 0));
    }

    private void Update()
    {
        SetPlayerAnimation();
    }

    private void InitPlayer()
    {
        animator = gameObject.GetComponentInChildren<Animator2D>();
        player_movement = gameObject.GetComponentInParent<PlayerMovement>();
        player_sensors = gameObject.GetComponentInParent<PlayerSensors>();
        player_stats = gameObject.GetComponent<PlayerStats>();
    }

    public void FocusCameraOnPlayer()
    {
        camera_follow_item.InstantFocus();
    }

    public void SetCameraBounds(CameraBounds bounds)
    {
        camera_follow_item.SetCameraBounds(bounds);
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

    private void AnimationDie()
    {
        if (animator != null)
        {
            animator.PlayAnimation("die", 0.06f, false);
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
        bool dead = player_stats.GetDead();

        if (!dead)
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
        else
        {
            AnimationDie();
        }

        was_dead = dead;
    }

    private Animator2D animator = null;
    private PlayerMovement player_movement = null;
    private PlayerSensors player_sensors = null;
    private PlayerStats player_stats = null;

    private CameraManager.CameraFollowItem camera_follow_item = null;

    private bool was_dead = false;
}
