using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelPlayerSensor();
public delegate void DelPlayerSensorGo(GameObject go);

public class PlayerSensors : MonoBehaviour
{
    private void Awake()
    {
        InitRigidBody();
    }

    private void Update()
    {
        CheckOnGroundAndMoveOnY();
        CheckTouchingWalls();

        DrawDebug();
    }

    private void InitRigidBody()
    {
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void DrawDebug()
    {
        for (int i = 0; i < player_foot_pos.Length; ++i)
        {
            GameObject curr_foot_pos = player_foot_pos[i];

            if (curr_foot_pos != null)
            {
                Vector3 on_air_vector_check = new Vector3(0, -platform_detectors_lenght_check);

                Color color = Color.green;

                if (!touching_platform)
                    color = Color.red;

                Debug.DrawLine(curr_foot_pos.transform.position, curr_foot_pos.transform.position + on_air_vector_check, color);
            }
        }

        for (int i = 0; i < player_lower_foot_pos.Length; ++i)
        {
            GameObject curr_lower_foot_pos = player_lower_foot_pos[i];

            if (curr_lower_foot_pos != null)
            {
                Vector3 on_air_vector_check = new Vector3(0, -platform_detectors_lenght_check);

                Color color = Color.green;

                if (!touching_platform)
                    color = Color.red;

                Debug.DrawLine(curr_lower_foot_pos.transform.position, curr_lower_foot_pos.transform.position + on_air_vector_check, color);
            }
        }

        for (int i = 0; i < player_left_pos.Length; ++i)
        {
            GameObject curr_left_pos = player_left_pos[i];

            if (curr_left_pos != null)
            {
                Vector3 left_vector_check = new Vector3(-platform_detectors_lenght_check, 0);

                Color color = Color.green;

                if (!touching_wall_left)
                    color = Color.red;

                Debug.DrawLine(curr_left_pos.transform.position, curr_left_pos.transform.position + left_vector_check, color);
            }
        }

        for (int i = 0; i < player_right_pos.Length; ++i)
        {
            GameObject curr_right_pos = player_right_pos[i];

            if (curr_right_pos != null)
            {
                Vector3 right_vector_check = new Vector3(platform_detectors_lenght_check, 0);

                Color color = Color.green;

                if (!touching_wall_right)
                    color = Color.red;

                Debug.DrawLine(curr_right_pos.transform.position, curr_right_pos.transform.position + right_vector_check, color);
            }
        }

    }

    private void CheckOnGroundAndMoveOnY()
    {
        if (player_foot_pos != null)
        {
            bool found = false;

            GameObject found_go = null;

            for (int i = 0; i < player_foot_pos.Length; ++i)
            {
                GameObject curr_foot_pos = player_foot_pos[i];

                if (curr_foot_pos != null)
                {
                    RaycastHit2D[] coll = Physics2D.RaycastAll(curr_foot_pos.transform.position,
                        Vector2.down, platform_detectors_lenght_check);

                    for (int y = 0; y < coll.Length; ++y)
                    {
                        RaycastHit2D curr_coll = coll[y];

                        Platform platform_script = curr_coll.collider.gameObject.transform.GetComponent<Platform>();

                        if (platform_script != null)
                        {
                            if (platform_script.GetUsable() && !curr_coll.collider.isTrigger)
                            {
                                standing_on_platform = platform_script;

                                found_go = platform_script.gameObject;

                                found = true;

                                break;
                            }
                        }
                    }
                }

                if (found)
                    break;
            }

            if (found)
            {
                if(!touching_platform)
                {
                    if (on_platform_start_touching != null)
                        on_platform_start_touching(found_go);

                    touching_platform = true;
                }
            }
            else
            {
                if (touching_platform)
                {
                    if (on_platform_stop_touching != null)
                        on_platform_stop_touching(standing_on_platform.gameObject);
                }


                standing_on_platform = null;

                touching_platform = false;
            }
        }

        if (rigid_body.velocity.y != 0)
            moving_on_y = true;
        else
            moving_on_y = false;
    }

    private void CheckTouchingWalls()
    {
        if (player_left_pos != null)
        {
            bool found = false;
            GameObject found_go = null;

            for (int i = 0; i < player_left_pos.Length; ++i)
            {
                GameObject curr_left_pos = player_left_pos[i];

                if (curr_left_pos != null)
                {
                    RaycastHit2D[] coll = Physics2D.RaycastAll(curr_left_pos.transform.position,
                        Vector2.left, platform_detectors_lenght_check);

                    for (int y = 0; y < coll.Length; ++y)
                    {
                        RaycastHit2D curr_coll = coll[y];

                        Wall wall_script = curr_coll.collider.gameObject.transform.GetComponent<Wall>();

                        if (wall_script != null && !curr_coll.collider.isTrigger)
                        {
                            found_go = wall_script.gameObject;
                            found = true;

                            break;
                        }
                    }
                }

                if (found)
                    break;
            }

            if (found)
            {
                if (!touching_wall_left)
                {
                    if (on_wall_start_touching != null)
                        on_wall_start_touching(found_go);
                }

                touching_wall_left = true;
            }
            else
                touching_wall_left = false;
        }

        if (player_right_pos != null)
        {
            bool found = false;
            GameObject found_go = null;

            for (int i = 0; i < player_right_pos.Length; ++i)
            {
                GameObject curr_right_pos = player_right_pos[i];

                if (curr_right_pos != null)
                {
                    RaycastHit2D[] coll = Physics2D.RaycastAll(curr_right_pos.transform.position,
                        Vector2.right, platform_detectors_lenght_check);

                    for (int y = 0; y < coll.Length; ++y)
                    {
                        RaycastHit2D curr_coll = coll[y];

                        Wall wall_script = curr_coll.collider.gameObject.transform.GetComponent<Wall>();

                        if (wall_script != null)
                        {
                            found_go = wall_script.gameObject;
                            found = true;

                            break;
                        }
                    }
                }

                if (found)
                    break;
            }

            if (found)
            {
                if(!touching_wall_right)
                {
                    if (on_wall_start_touching != null)
                        on_wall_start_touching(found_go);
                }

                touching_wall_right = true;
            }
            else
                touching_wall_right = false;
        }
    }

    public bool GetTouchingPlatform()
    {
        return touching_platform;
    }

    public bool GetTouchingWallLeft()
    {
        return touching_wall_left;
    }

    public bool GetTouchingWallRight()
    {
        return touching_wall_right;
    }

    public bool GetMovingOnY()
    {
        return moving_on_y;
    }

    public Platform GetStandingPlatform()
    {
        return standing_on_platform;
    }

    public Collider2D GetBodyCollider()
    {
        return body_collider;
    }

    public void SuscribeToOnWallStartTouching(DelPlayerSensorGo callback)
    {
        on_wall_start_touching += callback;
    }

    public void SuscribeToOnPlatformStartTouching(DelPlayerSensorGo callback)
    {
        on_platform_start_touching += callback;
    }

    public void SuscribeToOnPlatformStopTouching(DelPlayerSensorGo callback)
    {
        on_platform_start_touching += callback;
    }

    [SerializeField]
    private GameObject[] player_foot_pos = null;

    [SerializeField]
    private GameObject[] player_lower_foot_pos = null;

    [SerializeField]
    private GameObject[] player_left_pos = null;

    [SerializeField]
    private GameObject[] player_right_pos = null;

    [SerializeField]
    private Collider2D body_collider = null;

    private Rigidbody2D rigid_body = null;

    private readonly float platform_detectors_lenght_check = 0.1f;

    private bool touching_platform = true;
    private bool touching_wall_left = false;
    private bool touching_wall_right = false;
    private bool moving_on_y = false;

    private Platform standing_on_platform = null;

    private DelPlayerSensorGo on_wall_start_touching = null;
    private DelPlayerSensorGo on_platform_start_touching = null;
    private DelPlayerSensorGo on_platform_stop_touching = null;
}
