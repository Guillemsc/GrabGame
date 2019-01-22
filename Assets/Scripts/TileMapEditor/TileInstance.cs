using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInstance : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Sprite GetSprite()
    {
        Sprite ret = null;

        if (sr != null)
            ret = sr.sprite;

        return ret;
    }

    [SerializeField]
    private SpriteRenderer sr = null;

    private Vector2Int grid_pos = Vector2Int.zero;
}
