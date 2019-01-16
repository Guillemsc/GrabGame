using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected void InitInstance(T _instance)
    {
        if (_instance != null)
            instance = _instance;
    }

    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                return null;
            }
        }
    }

    public static bool Valid()
    {
        return instance != null;
    }

    private static T instance;
}

