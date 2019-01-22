using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    LevelManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        SetCurrLevel(starting_level);
    }

    public void SetCurrLevel(Level set)
    {
        if (set != null)
        {
            curr_level = set;
        }
    }

    public Level GetCurrLevel()
    {
        return curr_level;
    }

    [SerializeField]
    private Level starting_level = null;

    private Level curr_level = null;
}
