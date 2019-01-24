using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        PLAYER_STATE_GROUNDED,
        PLAYER_STATE_JUMPING,
    }

    private void Awake()
    {
        InitEvents();
        InitRigidBody();
        InitBodyCollisions();
        InitPlayer();
    }

    private void Update ()
    {
        if (movement_enabled)
        {
            GetPlayerInput();

            MovePlayerLeftRight();

            PlayerJump();

            PlayerGoThroughPlatforms();

            CapVelocity();
        }
    }

    private void InitEvents()
    {
        EventManager.Instance.Suscribe(OnEvent);
    }


    private void InitRigidBody()
    {
        rigid_body = gameObject.GetComponent<Rigidbody2D>();

        if (rigid_body != null)
        {
            rigid_body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigid_body.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    private void InitBodyCollisions()
    {
        CollisionDetector body_collisions = gameObject.GetComponentInChildren<CollisionDetector>();
    }

    private void InitPlayer()
    {
        player_state = PlayerState.PLAYER_STATE_GROUNDED;

        player_sensors = gameObject.GetComponent<PlayerSensors>();
        player_sensors.SuscribeToOnPlatformStartTouching(OnPlatformStartTouching);
        player_sensors.SuscribeToOnWallStartTouching(OnWallStartTouching);
        player_sensors.SuscribeToOnPlatformStopTouching(OnPlatformStopTouching);
        player_sensors.SuscribeToOnWallStopTouching(OnWallStopTouching);

        next_jump_timer.Start();
        next_jump_timer.AddTime(next_jump_min_time_sec);

        allow_jump_after_ground_detection_jump_timer.Start();
        allow_jump_after_ground_detection_jump_timer.AddTime(allow_jump_after_ground_detection_time_sec);
    }

    private void OnEvent(Event ev)
    {
        switch(ev.Type())
        {
            case EventType.EVENT_PLAYER_DIES:
                {
                    SetMovementEnabled(false);
                    ResetRigidBodyVelocity();
                    SetKinematic();

                    break;
                }
            case EventType.EVENT_PLAYER_REESPAWNS:
                {
                    SetMovementEnabled(true);
                    ResetRigidBodyVelocity();
                    SetDynamic();

                    break;
                }
            case EventType.EVENT_LEVEL_END:
                {
                    SetMovementEnabled(false);
                    ResetRigidBodyVelocity();

                    break;
                }
            case EventType.EVENT_LEVEL_START:
                {
                    SetMovementEnabled(true);
                    ResetRigidBodyVelocity();

                    break;
                }
        }
    }

    public void SetMovementEnabled(bool set)
    {
        movement_enabled = set;
    }

    public bool GetMovementEnabled()
    {
        return movement_enabled;
    }

    public void ResetRigidBodyVelocity()
    {
        rigid_body.velocity = new Vector2(0, 0);
    }

    public void SetKinematic()
    {
        rigid_body.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetDynamic()
    {
        rigid_body.bodyType = RigidbodyType2D.Dynamic;
    }

    public Rigidbody2D GetRigidBody()
    {
        return rigid_body;
    }

    private void GetPlayerInput()
    {
        input = Vector2Int.zero;

        if (Input.GetKey("a"))
            input.x -= 1;

        if (Input.GetKey("d"))
            input.x += 1;

        if (Input.GetKeyDown("w"))
            input.y = 1;
        else if(Input.GetKey("w"))
            input.y = 2;

        if (Input.GetKeyDown("s"))
            input.y = -1;
    }

    private void MovePlayerLeftRight()
    {
        bool x_acceleration = false;

        if (player_sensors.GetTouchingPlatform())
        {
            if (input.x != 0)
                x_acceleration = true;

            if (x_acceleration)
            {
                float x_run_acceleration_dt = x_run_acceleration * Time.deltaTime;

                if (input.x > 0)
                {
                    rigid_body.velocity += new Vector2(x_run_acceleration_dt, 0);
                }
                else
                {
                    rigid_body.velocity -= new Vector2(x_run_acceleration_dt, 0);
                }
            }
            else
            {
                float x_run_deceleration_dt = x_run_deceleration * Time.deltaTime;

                if (rigid_body.velocity.x > 0)
                {
                    if (rigid_body.velocity.x - x_run_deceleration_dt < 0)
                        x_run_deceleration_dt = rigid_body.velocity.x;

                    rigid_body.velocity -= new Vector2(x_run_deceleration_dt, 0);
                }
                else if (rigid_body.velocity.x < 0)
                {
                    if (rigid_body.velocity.x + x_run_deceleration_dt > 0)
                        x_run_deceleration_dt = -rigid_body.velocity.x;

                    rigid_body.velocity += new Vector2(x_run_deceleration_dt, 0);
                }
            }
        }
        else
        {
            if (input.x != 0)
                x_acceleration = true;

            if ((player_sensors.GetTouchingWallLeft() || player_sensors.GetTouchingWallRight())
                && !player_sensors.GetTouchingPlatform())
            {
                x_acceleration = false;
            }

            if (x_acceleration)
            {
                float x_jump_acceleration_dt = x_jump_acceleration * Time.deltaTime;
                float x_jump_deceleration_dt = x_jump_deceleration * Time.deltaTime;

                if (input.x > 0)
                {
                    if (rigid_body.velocity.x < 0)
                        rigid_body.velocity += new Vector2(x_jump_deceleration_dt, 0);

                    else
                        rigid_body.velocity += new Vector2(x_jump_acceleration_dt, 0);
                }
                else
                {
                    if (rigid_body.velocity.x > 0)
                        rigid_body.velocity -= new Vector2(x_jump_deceleration_dt, 0);

                    else
                        rigid_body.velocity -= new Vector2(x_jump_acceleration_dt, 0);
                }
            }
        }
    }


    private void PlayerJump()
    {
        if (next_jump_timer.ReadTime() > next_jump_min_time_sec)
        {
            if (allow_jump_after_ground_detection_jump_timer.ReadTime() > allow_jump_after_ground_detection_time_sec)
            {
                if (input.y == 1)
                {
                    if (player_sensors.GetTouchingPlatform())
                    {
                        player_state = PlayerState.PLAYER_STATE_JUMPING;

                        rigid_body.velocity = new Vector2(rigid_body.velocity.x, 0);
                        rigid_body.AddForce(Vector2.up * y_jump_force);

                        normal_jumping = true;

                        next_jump_timer.Start();
                    }
                    else if (player_sensors.GetTouchingWallLeft())
                    {
                        rigid_body.velocity = new Vector2(rigid_body.velocity.x, 0);
                        rigid_body.AddForce(Vector2.up * y_jump_force * y_jump_multiplier_on_wall);
                        rigid_body.AddForce(Vector2.right * y_jump_force * x_jump_multiplier_on_wall);

                        next_jump_timer.Start();
                    }
                    else if (player_sensors.GetTouchingWallRight())
                    {
                        rigid_body.velocity = new Vector2(rigid_body.velocity.x, 0);
                        rigid_body.AddForce(Vector2.up * y_jump_force * y_jump_multiplier_on_wall);
                        rigid_body.AddForce(Vector2.left * y_jump_force * x_jump_multiplier_on_wall);

                        next_jump_timer.Start();
                    }
                }   
            }
        }

        if (input.y <= 0)
        {
            if (normal_jumping)
            {
                float dt_x_jump_multiplier_on_not_pressing = x_jump_multiplier_on_not_pressing * Time.deltaTime;
                rigid_body.AddForce(Vector2.down * y_jump_force * dt_x_jump_multiplier_on_not_pressing);
            }
        }
    }

    private void PlayerGoThroughPlatforms()
    {
        if (input.y == -1)
        {
            if (player_sensors.GetTouchingPlatform())
            {
                Platform standing_platform = player_sensors.GetStandingPlatform();

                if (standing_platform != null)
                {
                    if (standing_platform.GetCanGoDown())
                    {
                        Physics2D.IgnoreCollision(player_sensors.GetBodyCollider(), standing_platform.GetCollider(), true);

                        platform_ignoring = standing_platform;
                    }
                }
            }
        }
    }

    private void CapVelocity()
    {
        if (rigid_body != null)
        {
            if (player_sensors.GetTouchingPlatform())
            {
                if (rigid_body.velocity.x > max_run_velocity)
                    rigid_body.velocity = new Vector2(max_run_velocity, rigid_body.velocity.y);

                else if (rigid_body.velocity.x < -max_run_velocity)
                    rigid_body.velocity = new Vector2(-max_run_velocity, rigid_body.velocity.y);
            }
            else
            {
                if (player_sensors.GetTouchingWallLeft() || player_sensors.GetTouchingWallRight())
                {
                    if (rigid_body.velocity.y < -max_fall_velocity_on_wall)
                        rigid_body.velocity = new Vector2(rigid_body.velocity.x, -max_fall_velocity_on_wall);
                }
                else
                {
                    if (rigid_body.velocity.y < -max_fall_velocity)
                        rigid_body.velocity = new Vector2(rigid_body.velocity.x, -max_fall_velocity);
                }

                if (rigid_body.velocity.x > max_x_velocity_on_air)
                    rigid_body.velocity = new Vector2(max_x_velocity_on_air, rigid_body.velocity.y);

                else if (rigid_body.velocity.x < -max_x_velocity_on_air)
                    rigid_body.velocity = new Vector2(-max_x_velocity_on_air, rigid_body.velocity.y);
            }
        }
    }

    private void OnWallStartTouching(GameObject go)
    {
        if(!player_sensors.GetTouchingPlatform())
            gameObject.transform.parent = go.transform;

        if (!player_sensors.GetTouchingPlatform()
            && (!player_sensors.GetTouchingWallLeft() && !player_sensors.GetTouchingWallRight())
            && rigid_body.velocity.y < 0)
        {
            rigid_body.velocity = new Vector2(rigid_body.velocity.x, 0);
        }

        normal_jumping = false;
    }

    private void OnWallStopTouching(GameObject go)
    {
        if(gameObject.transform.parent == go.transform)
            gameObject.transform.parent = null;
    }

    private void OnPlatformStartTouching(GameObject go)
    {
        gameObject.transform.parent = go.transform;

        allow_jump_after_ground_detection_jump_timer.Start();

        player_state = PlayerState.PLAYER_STATE_GROUNDED;

        normal_jumping = false;
    }

    private void OnPlatformStopTouching(GameObject go)
    {
        gameObject.transform.parent = null;

        if (platform_ignoring != null)
        {
            if (platform_ignoring.gameObject == go)
            {
                Physics2D.IgnoreCollision(player_sensors.GetBodyCollider(), platform_ignoring.GetCollider(), false);

                platform_ignoring = null;
            }
        }
    }

    public Vector2Int GetInput()
    {
        return input;
    }

    public Vector2 GetRigidBodyVelocity()
    {
        return rigid_body.velocity;
    }

    public void SetNormalJumpToFalse()
    {
        normal_jumping = false;
    }

    private bool movement_enabled = true;

    [SerializeField]
    private float max_run_velocity = 0.0f;

    [SerializeField]
    private float x_run_acceleration = 0.0f;

    [SerializeField]
    private float x_run_deceleration = 0.0f;

    [SerializeField]
    private float y_jump_force = 0.0f;

    [SerializeField]
    private float y_jump_multiplier_on_wall = 0.0f;

    [SerializeField]
    private float x_jump_multiplier_on_wall = 0.0f;

    [SerializeField]
    private float x_jump_multiplier_on_not_pressing = 0.0f;

    [SerializeField]
    private float x_jump_acceleration = 0.0f;

    [SerializeField]
    private float x_jump_deceleration = 0.0f;

    [SerializeField]
    private float max_x_velocity_on_air = 0.0f;

    [SerializeField]
    private float max_fall_velocity = 0.0f;

    [SerializeField]
    private float max_fall_velocity_on_wall = 0.0f;

    [SerializeField]
    private float next_jump_min_time_sec = 0.0f;

    [SerializeField]
    private float allow_jump_after_ground_detection_time_sec = 0.0f;

    private Vector2Int input = Vector2Int.zero;

    [SerializeField]
    private PlayerState player_state = new PlayerState();

    private PlayerSensors player_sensors = null;
    private Rigidbody2D rigid_body = null;

    private bool normal_jumping = false;

    private Timer next_jump_timer = new Timer();
    private Timer allow_jump_after_ground_detection_jump_timer = new Timer();

    private Platform platform_ignoring = null;
}
