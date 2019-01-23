using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabBrushPainterManager : Singleton<PrefabBrushPainterManager>
{
    PrefabBrushPainterManager()
    {
        InitInstance(this);
    }

    public void CreateGroup()
    {
        BrushGroup bg = new BrushGroup();
        bg.SetName("NewGroup");

        groups.Add(bg);
    }

    public void RemoveGroup(BrushGroup bg)
    {
        groups.Remove(bg);

        CheckUsingBrushExists();
    }

    public List<BrushGroup> GetGroups()
    {
        return groups;
    }

    public void SetUsingBrush(GridBrushItem set)
    {
        using_brush = set;

        #if UNITY_EDITOR

        burush_asset.SetPrefab(set.gameObject);

        #endif
    }

    public GridBrushItem GetUsingBrush()
    {
        return using_brush;
    }

    public void CheckUsingBrushExists()
    {
        bool exists = false;

        for(int i = 0; i < groups.Count; ++i)
        {
            BrushGroup curr_group = groups[i];

            List<GridBrushItem> items = curr_group.GetBrushes();

            for(int y = 0; y < items.Count; ++y)
            {
                if(items[y] == using_brush)
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
                break;
        }

        if (!exists)
        {
            using_brush = null;

            #if UNITY_EDITOR

            if(using_brush != null)
                burush_asset.SetPrefab(using_brush.gameObject);
            else
                burush_asset.SetPrefab(null);

            #endif
        }
    }

    [System.Serializable]
    public class BrushGroup
    {
        public void SetName(string set)
        {
            name = set;
        }

        public string GetName()
        {
            return name;
        }

        public void UpdateBrushes()
        {
            int count = wanted_brushes;

            for (int i = brushes.Count - 1; i < count; ++i)
            {
                brushes.Add(null);
            }

            if (count < brushes.Count)
            {
                brushes.RemoveRange(count, brushes.Count - count);
            }

            PrefabBrushPainterManager.Instance.CheckUsingBrushExists();
        }

        public void AddBrushObject(int index, GridBrushItem gbi)
        {
            if (brushes.Count > index)
            {
                brushes[index] = gbi;
            }
        }

        public void SetWantedBrushes(int wanted_brushes)
        {
            this.wanted_brushes = wanted_brushes;
        }

        public int GetWantedBrushes()
        {
            return wanted_brushes;
        }

        public List<GridBrushItem> GetBrushes()
        {
            return brushes;
        }

        [SerializeField]
        private string name = "";

        [SerializeField]
        private List<GridBrushItem> brushes = new List<GridBrushItem>();

        [SerializeField]
        private int wanted_brushes = 0;

        [SerializeField]
        public bool editor_foolded = false;
    }

    #if UNITY_EDITOR

    [SerializeField]
    private PrefabBrushPainter burush_asset = null;

    #endif

    [SerializeField] [HideInInspector]
    private List<BrushGroup> groups = new List<BrushGroup>();

    private GridBrushItem using_brush = null;
}
