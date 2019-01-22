using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileMapEditorManager : Singleton<TileMapEditorManager>
{
    TileMapEditorManager()
    {
        InitInstance(this);
    }

    public void CreateTileGroup()
    {
        TileGroup tg = new TileGroup();

        tg.SetName("NewGroup");

        tile_groups.Add(tg);
    }

    public void RemoveTileGroup(TileGroup tg)
    {
        if(tg != null)
        {
            tile_groups.Remove(tg);
        }
    }

    public List<TileGroup> GetTileGroups()
    {
        return tile_groups;
    }

    [System.Serializable]
    public class TileGroup
    {
        public void SetName(string set)
        {
            name = set;
        }

        public string GetName()
        {
            return name;
        }

        public List<TileInstance> GetTiles()
        {
            return tiles;
        }

        public void SetTiles(int count)
        {
            for (int i = tiles.Count - 1; i < count; ++i)
            {
                tiles.Add(null);
            }

            if (count < tiles.Count)
            {
                tiles.RemoveRange(count, tiles.Count - count);
            }
        }

        public void AddTile(int index, TileInstance ti)
        {
            if (tiles.Count > index)
            {
                tiles[index] = ti;
            }
        }

        public void RemoveTile(TileInstance ti)
        {
            if (ti != null)
            {
                tiles.Remove(ti);
            }
        }

        [SerializeField]
        private List<TileInstance> tiles = new List<TileInstance>();

        [SerializeField]
        private string name = "";

        [SerializeField]
        public bool editor_folded = false;
    }

    [SerializeField] [HideInInspector]
    private List<TileGroup> tile_groups = new List<TileGroup>();
}
