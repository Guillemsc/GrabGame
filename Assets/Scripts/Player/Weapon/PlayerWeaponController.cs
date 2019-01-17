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

        SetClosedHead();
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
                StopGrab(true);
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

            SetOpenedHead();

            player_movement.SetNormalJumpToFalse();

            shooting = true;
        }
    }

    private void UpdateWeaponShoot()
    {
        if(shooting && !grabbed)
        {
            float dt_shooting_speed = shooting_speed * Time.deltaTime;

            weapon_head_rotation_point.transform.localPosition += new Vector3(dt_shooting_speed, 0, 0);

            CheckSpawnTrail();

            float distance = Vector2.Distance(weapon_head_rotation_point.transform.position, gameObject.transform.position);

            if(Mathf.Abs(distance) > max_shoot_distance)
            {
                StopWeaponShoot();
            }
        }
    }

    private void StopWeaponShoot()
    {
        weapon_head_rotation_point.transform.localPosition = starting_head_pos;

        SetClosedHead();
        DeleteTrial();

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

            AdjustTrailDistance();
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

                if (distance_joint.distance > max_shoot_distance)
                    distance_joint.distance = max_shoot_distance;

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

            trails_starting_distance = distance_joint.distance;

            SetClosedHead();

            PlatformManager.Instance.DisableCollisions(player_sensors.GetBodyCollider());

            player_movement.SetMovementEnabled(false);

            CameraManager.Instance.CameraStartShake(CameraManager.Instance.GetUsedCamera(), 0.05f, 0.1f);

            grabbed = true;
        }
    }

    private void StopGrab(bool use_boost = false)
    {
        if (distance_joint != null)
        {
            Destroy(distance_joint);

            distance_joint = null;
        }

        weapon_head_rotation_point.transform.parent = rotation_point.transform;

        DeleteTrial();

        PlatformManager.Instance.EnableCollisions(player_sensors.GetBodyCollider());

        player_movement.SetMovementEnabled(true);

        if(use_boost && grabbed)
            rigid_body.AddForce(Vector2.up * grabbed_end_force);

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
                    StopRecoverintTowardsWeapon(true);
                }
            }
        }
    }

    private void StopRecoverintTowardsWeapon(bool use_boost = false)
    {
        recovering_towards_weapon = false;

        float distance_recovered = recovered_start_distance - recovered_curr_distance;

        if (use_boost)
        {
            rigid_body.velocity = new Vector2(0, 0);
            rigid_body.AddForce(Vector2.up * recover_end_force);
        }
    }

    private void SpawnTrail(float distance)
    {
        SpawnedTrail st = new SpawnedTrail();

        st.go = new GameObject();
        st.go.transform.parent = trails_parent.transform;
        st.go.transform.localPosition = new Vector3(-distance, 0, 0);
        st.go.transform.localEulerAngles = new Vector3(0, 0, 0);
        st.go.name = "Trail:" + spawned_trails.Count;
        SpriteRenderer sr = st.go.AddComponent<SpriteRenderer>();
        sr.sprite = weapon_trail_sprite;

        st.distance_from_player = distance;
        st.distance_from_weapon_head = Vector2.Distance(st.go.transform.position, weapon_head.transform.position);

        spawned_trails.Add(st);
    }

    private void CheckSpawnTrail()
    {
        if(shooting && !grabbed)
        {
            bool spawn = false;
            bool fist = false;
            float distance_spawn = 0;

            if(spawned_trails.Count == 0)
            {
                distance_spawn = 0.2f;

                spawn = true;
                fist = true;
            }
            else
            {
                SpawnedTrail last_trail = spawned_trails[spawned_trails.Count - 1];

                float distance = Vector2.Distance(last_trail.go.transform.position, 
                    trail_spawn_pos.transform.position);

                if(distance >= trails_spawn_distance)
                {
                    distance_spawn = last_trail.distance_from_player + trails_spawn_distance;

                    spawn = true;
                }
            }

            if(fist)
            {
                trails_parent = new GameObject();
                trails_parent.name = "TrailsParent";
                trails_parent.transform.parent = weapon_head.transform;
                trails_parent.transform.localEulerAngles = new Vector3(0, 0, 0);
                trails_parent.transform.localPosition = new Vector3(0, 0.025f, 0);
            }

            if (spawn)
            {
                SpawnTrail(distance_spawn);
            }
        }
    }

    private void AdjustTrailDistance()
    {
        if (shooting && grabbed)
        {
            float distance = Vector2.Distance(trail_spawn_pos.transform.position, weapon_head.transform.position);

            float position_offset = distance_joint.distance - trails_starting_distance;

            float max_distance = 0;

            for (int i = 0; i < spawned_trails.Count;)
            {
                SpawnedTrail curr_trail = spawned_trails[i];

                if (max_distance < curr_trail.distance_from_weapon_head)
                    max_distance = curr_trail.distance_from_weapon_head;

                if (distance < curr_trail.distance_from_weapon_head)
                {
                    Destroy(curr_trail.go);
                    spawned_trails.RemoveAt(i);
                }
                else
                    ++i;
            }
            
            while(distance - trails_spawn_distance > max_distance)
            {
                max_distance += trails_spawn_distance;

                SpawnTrail(max_distance);
            }
        }
    }

    private void DeleteTrial()
    {
        Destroy(trails_parent);
        trails_parent = null;
        spawned_trails.Clear();
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

    private void SetClosedHead()
    {
        head_sprite_renderer.sprite = weapon_head_closed_sprite;
    }

    private void SetOpenedHead()
    {
        head_sprite_renderer.sprite = weapon_head_opened_sprite;
    }

    public bool GetShooting()
    {
        return shooting;
    }

    public bool GetGrabbed()
    {
        return grabbed;
    }

    public class SpawnedTrail
    {
        public GameObject go = null;
        public float distance_from_player = 0.0f;
        public float distance_from_weapon_head = 0.0f;
    }

    [SerializeField]
    private Sprite weapon_head_closed_sprite = null;

    [SerializeField]
    private Sprite weapon_head_opened_sprite = null;

    [SerializeField]
    private Sprite weapon_trail_sprite = null;

    [SerializeField]
    private GameObject trail_spawn_pos = null;

    [SerializeField]
    private SpriteRenderer head_sprite_renderer = null;

    [SerializeField]
    private GameObject rotation_point = null;

    [SerializeField]
    private GameObject weapon_base = null;

    [SerializeField]
    private GameObject weapon_head = null;

    [SerializeField]
    private GameObject weapon_head_rotation_point = null;

    [SerializeField]
    private float max_shoot_distance = 0;

    [SerializeField]
    private float shooting_speed = 0;

    [SerializeField]
    private float grabbed_up_down_speed = 0;

    [SerializeField]
    private float grabbed_end_force = 0;

    [SerializeField]
    private float grabbed_x_acceleration = 0;

    [SerializeField]
    private float max_grabbed_x_velocity = 0;

    [SerializeField]
    private float recovering_towards_weapon_speed = 0;

    [SerializeField]
    private float recover_end_force = 0;

    [SerializeField]
    private float trails_spawn_distance = 0;

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

    private List<SpawnedTrail> spawned_trails = new List<SpawnedTrail>();
    private GameObject trails_parent = null;
    private float trails_starting_distance = 0;
}
