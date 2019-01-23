using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(PrefabBrushPainterManager))]
public class PrefabBrushPainterManagerEd : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        PrefabBrushPainterManager myScript = (PrefabBrushPainterManager)target;

        if (!Application.isPlaying)
        {
            if (GUILayout.Button("Add Group"))
            {
                myScript.CreateGroup();
            }
        }

        List<PrefabBrushPainterManager.BrushGroup> groups = myScript.GetGroups();

        for(int i = 0; i < groups.Count; ++i)
        {
            PrefabBrushPainterManager.BrushGroup curr_group = groups[i];

            string group_name = curr_group.GetName();

            curr_group.editor_foolded = EditorGUILayout.Foldout(curr_group.editor_foolded, group_name);

            if(curr_group.editor_foolded)
            {
                if (!Application.isPlaying)
                {
                    string new_name = GUILayout.TextField(group_name);

                    curr_group.SetName(new_name);
                }

                List<GridBrushItem> brushes = curr_group.GetBrushes();

                if (!Application.isPlaying)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("Brushes:", GUILayout.Width(50));

                    int new_brushes_count = EditorGUILayout.IntField("", curr_group.GetWantedBrushes(), GUILayout.Width(50));

                    if (new_brushes_count != brushes.Count)
                        curr_group.SetWantedBrushes(new_brushes_count);

                    if (GUILayout.Button("Update", GUILayout.Width(50)))
                        curr_group.UpdateBrushes();
                    
                    EditorGUILayout.EndHorizontal();
                }

                for (int y = 0; y < brushes.Count; ++y)
                {
                    GridBrushItem curr_brush = brushes[y];

                    string brush_name = "ItmNotSet";

                    if (curr_brush != null)
                        brush_name = curr_brush.name;

                    if (!Application.isPlaying)
                    {
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label(brush_name, GUILayout.Width(100));

                        GridBrushItem new_item = (GridBrushItem)EditorGUILayout.
                            ObjectField("", curr_brush, typeof(GridBrushItem), false, GUILayout.Width(50));

                        curr_group.AddBrushObject(y, new_item);

                        if (curr_brush != null)
                        {
                            Sprite used_sprite = curr_brush.GetSprite();

                            EditorGUI.BeginDisabledGroup(true);

                            if (used_sprite != null)
                            {
                                Texture2D prev = AssetPreview.GetAssetPreview(curr_brush.gameObject);
                                EditorGUILayout.ObjectField("", prev, typeof(Texture2D), false, GUILayout.Width(70));
                            }

                            EditorGUI.EndDisabledGroup();
                        }

                        //if (GUILayout.Button("Use", GUILayout.Width(70)))
                        //{

                        //    //GridBrushEditor a = new GridBrushEditor();
                        //    //a. = curr_brush.brush;
                        //}

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                        GUILayout.Label(brush_name);
                }
            }
        }
        
    }
}

#endif
