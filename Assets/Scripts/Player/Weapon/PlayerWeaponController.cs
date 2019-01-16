using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
	void Awake ()
    {
        InitWeaponHead();
        InitRigidBody();
        InitPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        WeaponMouseRotation();

        MouseInput();

        UpdateWeaponShoot();
        UpdateWeaponGrabbed();

        UpdateGrabbedInput();

        UpdateRecoveringTowardsWeapon();

        CapVelocity();
    }

    private void InitWeaponHead()
    {
        CollisionDetector cd = weapon_head.GetComponent<CollisionDetector>();
        cd.SuscribeOnTriggerEnter2D(OnHeadTriggerEnter);
    }

    private void InitRigidBody()
    {
        rigid_body = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    private void InitPlayer()
    {
        player_movement = gameObject.GetComponentInParent<PlayerMovement>();

        player_sensors = gameObject.GetComponentInParent<PlayerSensors>();
        player_sensors.SuscribeToOnPlatformStartTouching(OnPlatformStartTouching);

    }

    private void WeaponMouseRotation()
    {
        if (!grabbed)
        {
            Vector2 world_mouse_pos = CameraManager.Instance.GetMouseWorldPos(CameraManager.Instance.GetUsedCamera());

            float angle = Utils.AngleFromTwoPoints(rotation_point.transform.position, world_mouse_pos);

            rotation_point.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void MouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!shooting)
            {
                StartShot();
            }
            else
            {
                StopGrab();
                StopWeaponShoot();
                StopRecoverintTowardsWeapon();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            StartRecoveringTowardsWeapon();
        }
    }

    private void StartShot()
    {
        if(!shooting)
        {
            starting_head_pos = weapon_head_rotation_point.transform.localPosition;

            shooting = true;
        }
    }

    private void UpdateWeaponShoot()
    {
        if(shooting && !grabbed)
        {
            float dt_shooting_speed = shooting_speed * Time.deltaTime;

            weapon_head_rotation_point.transform.localPosition += new Vector3(dt_shooting_speed, 0, 0);
        }
    }

    private void StopWeaponShoot()
    {
        weapon_head_rotation_point.transform.localPosition = starting_head_pos;

        shooting = false;
    }

    private void UpdateWeaponGrabbed()
    {
        if(grabbed)
        {
            weapon_head_rotation_point.transform.position = grabbed_point;

            float angle = Utils.AngleFromTwoPoints(weapon_head_rotation_point.transform.position, gameObject.transform.position);
            weapon_head_rotation_point.transform.eulerAngles = new Vector3(0, 0, angle + 180);
            rotation_point.transform.eulerAngles = new Vector3(0, 0, angle + 180);
        }
    }

    private void UpdateGrabbedInput()
    {
        if(grabbed)
        {
            if(distance_joint != null && !recovering_towards_weapon)
            {
                Vector2Int input = Vector2Int.zero;

                if (Input.GetKey("w"))
                    input.y += 1;

                if (Input.GetKey("s"))
                    input.y -= 1;

                if (Input.GetKey("d"))
                    input.x += 1;

                if (Input.GetKey("a"))
                    input.x -= 1;

                if (input.y > 0)
                {
                    float dt_grabbed_up_down_speed = grabbed_up_down_speed * Time.deltaTime;
                    distance_joint.distance -= dt_grabbed_up_down_speed;
                }
                else if(input.y < 0)
                {
                    float dt_grabbed_up_down_speed = grabbed_up_down_speed * Time.deltaTime;
                    distance_joint.distance += dt_grabbed_up_down_speed;
                }

                float dt_grabbed_x_acceleration = grabbed_x_acceleration * Time.deltaTime;

                if (input.x > 0)
                {                   
                    rigid_body.velocity += new Vector2(dt_grabbed_x_acceleration, 0);                    
                }
                else if(input.x < 0)
                {
                    rigid_body.velocity -= new Vector2(dt_grabbed_x_acceleration, 0);
                }
            }
        }
    }

    private void CapVelocity()
    {
        if(grabbed)
        {
            if (rigid_body.velocity.x > max_grabbed_x_velocity)
                rigid_body.velocity = new Vector2(max_grabbed_x_velocity, rigid_body.velocity.y);

            else if (rigid_body.velocity.x < -max_grabbed_x_velocity)
                rigid_body.velocity = new Vector2(-max_grabbed_x_velocity, rigid_body.velocity.y);
        }
    }
            
    private void StartGrab()
    {
        if (shooting)
        {
            grabbed_point = weapon_head_rotation_point.transform.position;
            weapon_head_rotation_point.transform.parent = gameObject.transform;

            distance_joint = rigid_body.gameObject.AddComponent<DistanceJoint2D>();
            distance_joint.connectedAnchor = grabbed_point;
            distance_joint.enableCollision = true;

            PlatformManager.Instance.DisableCollisions(player_sensors.GetBodyCollider());

            player_movement.SetMovementEnabled(false);

            grabbed = true;
        }
    }

    private void StopGrab()
    {
        if (distance_joint != null)
        {
            Destroy(distance_joint);

            distance_joint = null;
        }

        weapon_head_rotation_point.transform.parent = rotation_point.transform;

        PlatformManager.Instance.EnableCollisions(player_sensors.GetBodyCollider());

        player_movement.SetMovementEnabled(true); 

        grabbed = false;
    }

    private void StartRecoveringTowardsWeapon()
    {
        if (grabbed)
        {
            if (!recovering_towards_weapon)
            {
                recovering_towards_weapon = true;

                recovered_start_distance = distance_joint.distance;
            }
        }
    }

    private void UpdateRecoveringTowardsWeapon()
    {
        if (recovering_towards_weapon)
        {
            if (distance_joint != null)
            {
                float dt_recovering_towards_weapon_speed = recovering_towards_weapon_speed * Time.deltaTime;

                distance_joint.distance -= dt_recovering_towards_weapon_speed;

                recovered_curr_distance = distance_joint.distance;

                if (distance_joint.distance <= 0.3)
                {
                    StopGrab();
                    StopWeaponShoot();
                    StopRecoverintTowardsWeapon();
                }
            }
        }
    }

    private void StopRecoverintTowardsWeapon()
    {
        recovering_towards_weapon = false;

        float distance_recovered = recovered_start_distance - recovered_curr_distance;

        rigid_body.AddForce(Vector2.up * recover_end_force);
    }

    private void OnHeadTriggerEnter(Collider2D coll)
    {
        if (!grabbed)
        {
            GrabablePoint gp = coll.gameObject.GetComponent<GrabablePoint>();

            if (gp != null)
            {
                StartGrab();
            }
        }
    }

    private void OnPlatformStartTouching(GameObject go)
    {
        if(!player_movement.GetMovementEnabled() && !grabbed)
        {
            player_movement.SetMovementEnabled(true);
        }
    }

    [SerializeField]
    private GameObject rotation_point = null;

    [SerializeField]
    private GameObject weapon_base = null;

    [SerializeField]
    private GameObject weapon_head = null;

    [SerializeField]
    private GameObject weapon_head_rotation_point = null;

    [SerializeField]
    private float shooting_speed = 0;

    [SerializeField]
    private float grabbed_up_down_speed = 0;

    [SerializeField]
    private float grabbed_x_acceleration = 0;

    [SerializeField]
    private float max_grabbed_x_velocity = 0;

    [SerializeField]
    private float recovering_towards_weapon_speed = 0;

    [SerializeField]
    private float recover_end_force = 0;

    private PlayerMovement player_movement = null;
    private PlayerSensors player_sensors = null;
    private Rigidbody2D rigid_body = null;

    private Vector3 starting_head_pos = Vector3.zero;
    private bool shooting = false;

    private Vector3 grabbed_point = Vector3.zero;
    private bool grabbed = false;
    private DistanceJoint2D distance_joint = null;

    private bool recovering_towards_weapon = false;
    private float recovered_start_distance = 0;
    private float recovered_curr_distance = 0;
}
