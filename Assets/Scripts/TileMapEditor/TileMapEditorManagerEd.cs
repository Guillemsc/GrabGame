using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(TileMapEditorManager))]
public class TileMapEditorManagerEd : Editor
{
    public override void OnInspectorGUI()
    {
        TileMapEditorManager myScript = (TileMapEditorManager)target;

        if (!Application.isPlaying)
        {
            if (GUILayout.Button("Add Group"))
            {
                myScript.CreateTileGroup();
            }
        }

        List<TileMapEditorManager.TileGroup> tile_groups = myScript.GetTileGroups();

        for (int i = 0; i < tile_groups.Count; ++i)
        {
            EditorGUI.indentLevel = 0;

            TileMapEditorManager.TileGroup curr_tile_group = tile_groups[i];

            string curr_name = curr_tile_group.GetName();

            curr_tile_group.editor_folded = EditorGUILayout.Foldout(curr_tile_group.editor_folded, curr_name);

            if (curr_tile_group.editor_folded)
            {
                EditorGUI.indentLevel = 1;

                if (!Application.isPlaying)
                {
                    GUILayout.BeginHorizontal();

                    string new_name = GUILayout.TextField(curr_name);

                    curr_tile_group.SetName(new_name);

                    if (GUILayout.Button("Remove"))
                    {
                        myScript.RemoveTileGroup(curr_tile_group);
                        break;
                    }

                    GUILayout.EndHorizontal();
                }

                List<TileInstance> tile_instances = curr_tile_group.GetTiles();

                if (!Application.isPlaying)
                {
                    int tiles_count = EditorGUILayout.IntField("Tiles:", tile_instances.Count);

                    if (tiles_count != tile_instances.Count)
                        curr_tile_group.SetTiles(tiles_count);
                }
                else
                    GUILayout.Label("Tiles: " + tile_instances.Count); 

                for (int y = 0; y < tile_instances.Count; ++y)
                {
                    EditorGUI.indentLevel = 3;

                    TileInstance curr_tile = tile_instances[y];

                    string name = y.ToString();

                    if (curr_tile != null)
                        name = curr_tile.gameObject.name;

                    if (!Application.isPlaying)
                    {
                        GUILayout.BeginHorizontal();

                        TileInstance new_tile = (TileInstance)EditorGUILayout.ObjectField(name, curr_tile,
                            typeof(TileInstance), false);

                        curr_tile_group.AddTile(y, new_tile);

                        if (curr_tile != null)
                        {
                            Sprite sp = curr_tile.GetSprite();

                            if (sp != null)
                            {
                                EditorGUI.BeginDisabledGroup(true);

                                EditorGUILayout.ObjectField(name, sp, typeof(Sprite), false);

                                EditorGUI.EndDisabledGroup();
                            }
                        }

                        GUILayout.EndHorizontal();
                    }

                    else
                        GUILayout.Label(name);
                }
            }
        }
    }
}

#endif
