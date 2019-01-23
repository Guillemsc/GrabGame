using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PrefabBrushPainterInstance : MonoBehaviour
{
    private void Update()
    {
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}

