using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmoke : MonoBehaviour
{
    private void Awake()
    {
        InitPlayer();

        InitVariables();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckWalkSmoke();
        CheckLandSmoke();
        CheckWallSlideSmoke();
        CheckWallJumpSmoke();
    }

    private void InitPlayer()
    {
        player_sensors = gameObject.GetComponentInParent<PlayerSensors>();
        player_movement = gameObject.GetComponentInParent<PlayerMovement>();
    }

    private void InitVariables()
    {
        walk_smoke_timer.Start();
        walk_smoke_timer.AddTime(walk_smoke_min_spawn_time);

        land_smoke_timer.Start();
        land_smoke_timer.AddTime(land_smoke_min_spawn_time);

        wall_slide_smoke_timer.Start();
        wall_slide_smoke_timer.AddTime(wall_slide_smoke_min_spawn_time);

        wall_jump_smoke_timer.Start();
        wall_jump_smoke_timer.AddTime(wall_jump_smoke_min_spawn_time);
    }

    private void CheckWalkSmoke()
    {
        if (!walk_smoke_enabled)
        {            
            if (player_movement.GetRigidBodyVelocity().x == 0)
            {
                walk_smoke_enabled = true;
            }

            if (player_movement.GetRigidBodyVelocity().x > 0)
            {
                if (last_direction_x != 1)
                    walk_smoke_enabled = true;

                last_direction_x = 1;
            }
            else if (player_movement.GetRigidBodyVelocity().x < 0)
            {
                if (last_direction_x != -1)
                    walk_smoke_enabled = true;

                last_direction_x = -1;
            }
            
        }

        if (!player_sensors.GetTouchingPlatform() || player_movement.GetRigidBodyVelocity().x == 0
            || weapon_controller.GetGrabbed())
            walk_smoke_enabled = false;

        if (walk_smoke_enabled)
        {
            walk_smoke_enabled = false;

            if (walk_smoke_timer.ReadTime() > walk_smoke_min_spawn_time)
            {
                SpawnWalkSmoke();

                walk_smoke_timer.Start();
            }
        }
    }

    private void CheckLandSmoke()
    {
        if (!last_grounded && player_sensors.GetTouchingPlatform())
            land_smoke_enabled = true;

        else if (last_grounded && !player_sensors.GetTouchingPlatform())
            land_smoke_enabled = true;

        last_grounded = player_sensors.GetTouchingPlatform();

        if(land_smoke_enabled)
        {
            land_smoke_enabled = false;

            if(land_smoke_timer.ReadTime() > land_smoke_min_spawn_time)
            {
                SpawnLandSmoke();

                land_smoke_timer.Start();
            }
        }
    }

    private void CheckWallSlideSmoke()
    {
        if(player_sensors.GetTouchingWallRight() && !player_sensors.GetTouchingPlatform())
        {
            if (player_movement.GetRigidBodyVelocity().y < 0)
            {
                wall_right_slide_smoke_enabled = true;
            }
        }

        if (player_sensors.GetTouchingWallLeft() && !player_sensors.GetTouchingPlatform())
        {
            if(player_movement.GetRigidBodyVelocity().y < 0)
                wall_left_slide_smoke_enabled = true;
        }

        if (wall_left_slide_smoke_enabled)
        {
            wall_left_slide_smoke_enabled = false;

            if(wall_slide_smoke_timer.ReadTime() > wall_slide_smoke_min_spawn_time)
            {
                SpawnWallSlideSmoke(true);

                wall_slide_smoke_timer.Start();
            }
        }

        if (wall_right_slide_smoke_enabled)
        {
            wall_right_slide_smoke_enabled = false;

            if (wall_slide_smoke_timer.ReadTime() > wall_slide_smoke_min_spawn_time)
            {
                float distance = Mathf.Abs(Vector2.Distance(player_sensors.gameObject.transform.position, last_slided_point));

                if (distance > wall_slide_smoke_min_distance)
                {
                    SpawnWallSlideSmoke(false);

                    wall_slide_smoke_timer.Start();

                    last_slided_point = player_sensors.gameObject.transform.position;
                }
            }
        }
    }

    private void CheckWallJumpSmoke()
    {
        if (last_touching_wall_left && !player_sensors.GetTouchingPlatform() && !player_sensors.GetTouchingWallLeft())
            wall_left_jump_smoke_enabled = true;

        last_touching_wall_left = player_sensors.GetTouchingWallLeft();

        if (last_touching_wall_right && !player_sensors.GetTouchingPlatform() && !player_sensors.GetTouchingWallRight())
            wall_right_jump_smoke_enabled = true;

        last_touching_wall_right = player_sensors.GetTouchingWallRight();

        if (wall_left_jump_smoke_enabled)
        {
            wall_left_jump_smoke_enabled = false;

            if (wall_jump_smoke_timer.ReadTime() > wall_jump_smoke_min_spawn_time)
            {
                SpawnWallJumpSmoke(true);

                wall_jump_smoke_timer.Start();
            }
        }

        if (wall_right_jump_smoke_enabled)
        {
            wall_right_jump_smoke_enabled = false;

            if (wall_jump_smoke_timer.ReadTime() > wall_jump_smoke_min_spawn_time)
            {
                SpawnWallJumpSmoke(false);

                wall_jump_smoke_timer.Start();
            }
        }
    }

    private void SpawnWalkSmoke()
    {
        Instantiate(walk_smoke, walk_smoke_pos.transform.position, Quaternion.identity);
    }

    private void SpawnLandSmoke()
    {
        Instantiate(land_smoke, land_smoke_pos.transform.position, Quaternion.identity);
    }

    private void SpawnWallSlideSmoke(bool mirror)
    {
        Vector3 spawn_pos = wall_right_slide_smoke_pos.transform.position;

        if(mirror)
            spawn_pos = wall_left_slide_smoke_pos.transform.position;

        GameObject obj = Instantiate(wall_slide_smoke, spawn_pos, Quaternion.identity);
        Animator2D anim = obj.GetComponentInChildren<Animator2D>();

        if (anim != null && mirror)
            anim.SetFilpX(true);
    }

    private void SpawnWallJumpSmoke(bool mirror)
    {
        Vector3 spawn_pos = wall_right_slide_smoke_pos.transform.position;

        if (mirror)
            spawn_pos = wall_left_slide_smoke_pos.transform.position;

        GameObject obj = Instantiate(wall_jump_smoke, spawn_pos, Quaternion.identity);
        Animator2D anim = obj.GetComponentInChildren<Animator2D>();

        if (anim != null && mirror)
            anim.SetFilpX(true);
    }

    [SerializeField]
    private PlayerWeaponController weapon_controller = null;

    [SerializeField]
    private GameObject walk_smoke = null;

    [SerializeField]
    private GameObject walk_smoke_pos = null;

    [SerializeField]
    private float walk_smoke_min_spawn_time = 0.0f;

    [SerializeField]
    private GameObject land_smoke = null;

    [SerializeField]
    private GameObject land_smoke_pos = null;

    [SerializeField]
    private float land_smoke_min_spawn_time = 0.0f;

    [SerializeField]
    private GameObject wall_slide_smoke = null;

    [SerializeField]
    private GameObject wall_left_slide_smoke_pos = null;

    [SerializeField]
    private GameObject wall_right_slide_smoke_pos = null;

    [SerializeField]
    private float wall_slide_smoke_min_spawn_time = 0.0f;

    [SerializeField]
    private float wall_slide_smoke_min_distance = 0;

    [SerializeField]
    private GameObject wall_jump_smoke = null;

    [SerializeField]
    private float wall_jump_smoke_min_spawn_time = 0.0f;

    private PlayerSensors player_sensors = null;
    private PlayerMovement player_movement = null;

    private bool walk_smoke_enabled = false;
    private int last_direction_x = 0;
    private Timer walk_smoke_timer = new Timer();

    private bool land_smoke_enabled = false;
    private bool last_grounded = true;
    private Timer land_smoke_timer = new Timer();

    private bool wall_left_slide_smoke_enabled = false;
    private bool wall_right_slide_smoke_enabled = false;
    private Vector2 last_slided_point = new Vector2();
    private Timer wall_slide_smoke_timer = new Timer();

    private bool wall_left_jump_smoke_enabled = false;
    private bool wall_right_jump_smoke_enabled = false;
    private bool last_touching_wall_left = true;
    private bool last_touching_wall_right = true;
    private Timer wall_jump_smoke_timer = new Timer();
}
