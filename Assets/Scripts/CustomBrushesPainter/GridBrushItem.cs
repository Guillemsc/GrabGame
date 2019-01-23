using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridBrushItem : MonoBehaviour
{
    public Sprite GetSprite()
    {
        Sprite ret = null;

        if (renderer != null)
            ret = renderer.sprite;

        return ret;
    }

    [SerializeField]
    private SpriteRenderer renderer = null;
}
