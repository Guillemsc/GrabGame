using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class TileMapInstance : MonoBehaviour
{
    private void Awake()
    {
        InitData();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            InitData();
    }

    private void InitData()
    {
        grid = gameObject.GetComponent<Grid>();

        if (grid == null)
            grid = gameObject.AddComponent<Grid>();

        tilemap = gameObject.GetComponentInChildren<Tilemap>();

        if(tilemap == null)
        {
            if(gameObject.transform.childCount == 0)
            {
                GameObject go = new GameObject();
                go.name = "Tilemap";
                go.transform.parent = transform;

                tilemap = go.AddComponent<Tilemap>();
            }
            else
            {
                GameObject child = gameObject.transform.GetChild(0).gameObject;

                tilemap = child.AddComponent<Tilemap>();
            }
        }
    }

    public void SetEditing(bool set)
    {
        editing = set;
    }

    public bool GetEditing()
    {
        return editing;
    }

    public bool TilePosHasTile(Vector2Int tile_pos)
    {
        bool ret = false;

        if(tile_maps.ContainsKey(tile_pos))
        {
            if(tile_maps[tile_pos] != null)
                ret = true;
        }

        return ret;
    }

    public TileMapInstance TilePosGetTile(Vector2Int tile_pos)
    {
        return tile_maps[tile_pos];
    }

    private Grid grid = null;
    private Tilemap tilemap = null;

    private bool editing = false;

    [SerializeField] [HideInInspector] 
    private Dictionary<Vector2Int, TileMapInstance> tile_maps = new Dictionary<Vector2Int, TileMapInstance>();
}
