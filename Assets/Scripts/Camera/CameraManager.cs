﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera camera;

    CameraManager()
    {
        InitInstance(this);
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        UpdateCameraFollow();
        UpdateCameraShake();
    }

    public Camera GetUsedCamera()
    {
        return camera;
    }

    public Vector3 GetMouseWorldPos(Camera cam)
    {
        Vector3 ret = Vector3.zero;

        if(cam != null)
            ret = cam.ScreenToWorldPoint(Input.mousePosition);

        return ret;
    }

    public void SetHorizontalSplitScreen(Camera left_camera, Camera right_camera)
    {
        if(left_camera != null && right_camera != null)
        {
            left_camera.rect = new Rect(0, 0, 0.5f, 1);
            right_camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    public void SetVerticalSplitScreen(Camera up_camera, Camera down_camera)
    {
        if (up_camera != null && down_camera != null)
        {
            up_camera.rect = new Rect(0, 0.5f, 1, 0.5f);
            down_camera.rect = new Rect(0, 0, 1, 0.5f);
        }
    }

    public void CameraFollow(Camera camera, GameObject go, float movement_time, Vector3 offset)
    {
        if (go != null && camera != null)
        {
            CameraStopFollow(camera);
            
            CameraFollowItem item = new CameraFollowItem(camera, go, movement_time, offset);
            following_items.Add(item);
        }
    }

    public void CameraUpdateFollow(Camera camera, float movement_time, Vector3 offset)
    {
        for (int i = 0; i < following_items.Count; ++i)
        {
            CameraFollowItem curr_item = following_items[i];

            if (curr_item.GetCamera() == camera)
            {
                curr_item.SetMovementTime(movement_time);
                curr_item.SetOffset(offset);
            }
        }
    }

    public void CameraStopFollow(Camera camera)
    {
        for (int i = 0; i < following_items.Count; ++i)
        {
            CameraFollowItem curr_item = following_items[i];

            if (curr_item.GetCamera() == camera)
            {
                following_items.RemoveAt(i);

                break;
            }
        }
    }

    public void CamerasStopFollow(GameObject go)
    {
        for (int i = 0; i < following_items.Count;)
        {
            CameraFollowItem curr_item = following_items[i];

            if (curr_item.GetFollowingGameObject() == go)
            {
                following_items.RemoveAt(i);
            }
            else
                ++i;
        }
    }

    public void CameraStartShake(Camera camera, float shake_duration, float shake_amount)
    {
        for(int i = 0; i < camera_shakes.Count; ++i)
        {
            if(camera_shakes[i].GetCamera() == camera)
            {
                camera_shakes.RemoveAt(i);
                break;
            }
        }

        CameraShake cs = new CameraShake(camera, shake_duration, shake_amount);
        cs.GetTimer().Start();

        camera_shakes.Add(cs);
    }

    private void UpdateCameraFollow()
    {
        for (int i = 0; i < following_items.Count; ++i)
        {
            CameraFollowItem curr_item = following_items[i];

            float movement_time = curr_item.GetMovementTime();
            Vector3 offset = curr_item.GetOffset();
            Camera camera = curr_item.GetCamera();
            Transform following_trans = curr_item.GetFollowingGameObject().transform;

            Vector3 desired_pos = following_trans.position + offset;

            Vector3 smoothed_position = Vector3.SmoothDamp(camera.transform.position, desired_pos, ref curr_item.velocity, movement_time);

            if (!camera.orthographic)
                camera.transform.position = smoothed_position;
            else
                camera.transform.position = new Vector3(smoothed_position.x, smoothed_position.y, camera.transform.position.z);
        }
    }

    private void UpdateCameraShake()
    {
        for(int i = 0; i < camera_shakes.Count;)
        {
            CameraShake curr_shake = camera_shakes[i];

            curr_shake.GetCamera().transform.position += Random.insideUnitSphere * curr_shake.GetShakeAmount();

            if (curr_shake.GetTimer().ReadTime() > curr_shake.GetShakeTime())
                camera_shakes.RemoveAt(i);
            else
                ++i;
        }
    }

    public class CameraFollowItem
    {
        public CameraFollowItem(Camera camera, GameObject to_follow, float movement_time, Vector3 offset)
        {
            this.camera = camera;
            this.to_follow = to_follow;
            this.movement_time = movement_time;
            this.offset = offset;
        }

        public GameObject GetFollowingGameObject()
        {
            return to_follow;
        }

        public Camera GetCamera()
        {
            return camera;
        }

        public void SetMovementTime(float movement_time)
        {
            if(movement_time >= 0)
                this.movement_time = movement_time;
        }

        public float GetMovementTime()
        {
            return movement_time;
        }

        public void SetOffset(Vector3 offset)
        {
            this.offset = offset;
        }

        public Vector3 GetOffset()
        {
            return offset;
        }

        private Camera camera = null;
        private GameObject to_follow = null;
        private float movement_time = 0.0f;
        private Vector3 offset = Vector3.zero;

        public Vector3 velocity = Vector3.zero;
    }

    public class CameraShake
    {
        public CameraShake(Camera cam, float shake_duration, float shake_amount)
        {
            this.cam = cam;   
            this.shake_duration = shake_duration;
            this.shake_amount = shake_amount;
        }

        public Camera GetCamera()
        {
            return cam;
        }

        public Timer GetTimer()
        {
            return shake_timer;
        }

        public float GetShakeTime()
        {
            return shake_duration;
        }

        public float GetShakeAmount()
        {
            return shake_amount;
        }

        private Camera cam = null;

        private Timer shake_timer = new Timer();
        private float shake_duration = 0.0f;

        private float shake_amount = 0.7f;
    }

    private List<CameraFollowItem> following_items = new List<CameraFollowItem>();
    private List<CameraShake> camera_shakes = new List<CameraShake>();
}