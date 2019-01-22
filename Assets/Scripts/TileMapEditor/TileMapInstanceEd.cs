using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(TileMapInstance))]
public class TileMapInstanceEd : Editor
{
    public override void OnInspectorGUI()
    {
        TileMapInstance myScript = (TileMapInstance)target;

        if (TileMapEditorManager.Instance != null)
        {
            List<TileMapEditorManager.TileGroup> tile_groups = TileMapEditorManager.Instance.GetTileGroups();

            for (int i = 0; i < tile_groups.Count; ++i)
            {
                EditorGUI.indentLevel = 0;

                TileMapEditorManager.TileGroup curr_tile_group = tile_groups[i];

                string curr_name = curr_tile_group.GetName();

                curr_tile_group.editor_folded = EditorGUILayout.Foldout(curr_tile_group.editor_folded, curr_name);

                if (curr_tile_group.editor_folded)
                {
                    EditorGUI.indentLevel = 1;

                    List<TileInstance> tile_instances = curr_tile_group.GetTiles();

                    GUILayout.Label("Tiles: " + tile_instances.Count);

                    for (int y = 0; y < tile_instances.Count; ++y)
                    {
                        EditorGUI.indentLevel = 2;

                        TileInstance curr_tile = tile_instances[y];

                        string name = y.ToString();

                        if (curr_tile != null)
                            name = curr_tile.gameObject.name;
                        
                        GUILayout.BeginHorizontal();

                        if (GUILayout.Button("Select"))
                        {
                            //myScript.CreateTileGroup();
                        }

                        GUILayout.Label(name);

                        if (curr_tile != null)
                        {
                            Sprite sp = curr_tile.GetSprite();

                            if (sp != null)
                            {
                                EditorGUI.BeginDisabledGroup(true);

                                EditorGUILayout.ObjectField("", sp, typeof(Sprite), false);

                                EditorGUI.EndDisabledGroup();
                            }
                        }

                        GUILayout.EndHorizontal(); 
                    }
                }
            }
        }
    }
}

#endif
