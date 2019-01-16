using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : Singleton<PlatformManager>
{
    PlatformManager()
    {
        InitInstance(this);
    }

    public void AddPlatform(Platform plat)
    {
        if (plat != null)
            platforms.Add(plat);
    }

    public void RemovePlatform(Platform plat)
    {
        if (plat != null)
            platforms.Remove(plat);
    }

    public void DisableCollisions(Collider2D coll)
    {
        if(coll != null)
        {
            for(int i = 0; i < platforms.Count; ++i)
            {
                Physics2D.IgnoreCollision(coll, platforms[i].GetCollider());
            }
        }
    }

    public void EnableCollisions(Collider2D coll)
    {
        if (coll != null)
        {
            for (int i = 0; i < platforms.Count; ++i)
            {
                Physics2D.IgnoreCollision(coll, platforms[i].GetCollider(), false);
            }
        }
    }

    List<Platform> platforms = new List<Platform>();
}
