using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static float AngleFromTwoPoints(Vector2 p1, Vector2 p2)
    {
        float xDiff = p2.x - p1.x;
        float yDiff = p2.y - p1.y;
        return Mathf.Atan2(yDiff, xDiff) * 180.0f / Mathf.PI;
    }

    public static int LayerFromName(string layer_name)
    {
        return LayerMask.NameToLayer(layer_name);
    }
}
