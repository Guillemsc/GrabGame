using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(PrefabBrushPainterInstance))]
public class PrefabBrushPainterInstanceEd : Editor
{
    public override void OnInspectorGUI()
    {
        PrefabBrushPainterInstance myScript = (PrefabBrushPainterInstance)target;

        List<PrefabBrushPainterManager.BrushGroup> groups = PrefabBrushPainterManager.Instance.GetGroups();

        for (int i = 0; i < groups.Count; ++i)
        {
            PrefabBrushPainterManager.BrushGroup curr_group = groups[i];

            string group_name = curr_group.GetName();

            curr_group.editor_foolded = EditorGUILayout.Foldout(curr_group.editor_foolded, group_name);

            if (curr_group.editor_foolded)
            {
                List<GridBrushItem> brushes = curr_group.GetBrushes();

                if (!Application.isPlaying)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("Brushes:", GUILayout.Width(50));

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

                        if (curr_brush != null)
                        {
                            GUILayout.Label(brush_name, GUILayout.Width(100));

                            Sprite used_sprite = curr_brush.GetSprite();

                            EditorGUI.BeginDisabledGroup(true);

                            if (used_sprite != null)
                            {
                                EditorGUILayout.ObjectField("", used_sprite, typeof(Sprite), false, GUILayout.Width(70));
                            }

                            EditorGUI.EndDisabledGroup();

                            var oldColor = GUI.backgroundColor;

                            if (PrefabBrushPainterManager.Instance.GetUsingBrush() == curr_brush && curr_brush != null)
                            {
                                GUI.backgroundColor = Color.green;
                            }

                            if (GUILayout.Button("Use", GUILayout.Width(70)))
                            {
                                PrefabBrushPainterManager.Instance.SetUsingBrush(curr_brush);
                            }

                            GUI.backgroundColor = oldColor;
                        }

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
