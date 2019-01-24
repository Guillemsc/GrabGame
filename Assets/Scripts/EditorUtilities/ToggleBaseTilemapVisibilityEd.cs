using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToggleBaseTilemapVisibilityEd : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            if(visible != last_visible)
            {
                ToggleVisibility(visible);
            }

            last_visible = visible;
        }
    }

    private void ToggleVisibility(bool visible)
    {
        int childs_count = gameObject.transform.childCount;

        for(int i = 0; i < childs_count; ++i)
        {
            GameObject curr_child = gameObject.transform.GetChild(i).gameObject;

            SpriteRenderer[] sprites = curr_child.GetComponentsInChildren<SpriteRenderer>();

            for(int y = 0; y < sprites.Length; ++y)
            {
                sprites[y].enabled = visible;
            }
        }
    }

    [SerializeField]
    private bool visible = false;

    [SerializeField] [HideInInspector]
    private bool last_visible = false;
}
