using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    [Header("Build")]
    [SerializeField] private bool is_release = false;

    [SerializeField] private string version = "0.1";

    [Header("Framerate")]
    [SerializeField] private bool vsync = true;

    [SerializeField] private int max_fps = 999;

    private bool paused = false;
    private float last_time_scale = 1.0f;

    AppManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        InitLibraries();
    }

    private void Start()
    {
        if (!is_release)
            SetVSync(vsync);

        SetMaxFPS(max_fps);
    }

    private void InitLibraries()
    {

    }

    public bool GetIsRelease()
    {
        return is_release;
    }

    public string GetVersion()
    {
        return version;
    }

    public void SetVSync(bool set)
    {
        vsync = set;
        QualitySettings.vSyncCount = (set == true ? 1 : 0);
    }

    public bool GetVSync()
    {
        return QualitySettings.vSyncCount == 1;
    }

    public void SetMaxFPS(int set)
    {
        if (max_fps > 0)
        {
            max_fps = set;
            Application.targetFrameRate = set;
        }
    }

    public int GetMaxFPS()
    {
        return Application.targetFrameRate;
    }

    public void Pause()
    {
        if (!paused)
        {
            last_time_scale = Time.timeScale;
            Time.timeScale = 0.0f;

            paused = true;
        }
    }

    public void Resume()
    {
        if (paused)
        {
            Time.timeScale = last_time_scale;

            paused = false;
        }
    }

    public bool GetPaused()
    {
        return paused;
    }
}
