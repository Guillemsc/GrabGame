using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR 

namespace UnityEditor
{
    [CreateAssetMenu(fileName = "Prefab brush", menuName = "Brushes/PrefabBrushPainter")]
    [CustomGridBrush(false, true, false, "Prefab Brush")]
    public class PrefabBrushPainter : GridBrush
    {
        private const float k_PerlinOffset = 100000f;
        private GameObject prefab = null;
        private GameObject preview_go = null;
        private float m_PerlinScale = 0.5f;
        private int m_Z;
        private GameObject prev_brushTarget;
        private Vector3Int prev_position;
        private bool tool_active = false;

        public void SetPrefab(GameObject prefab)
        {
            bool prefab_is_different = false;

            if (prefab != this.prefab)
                prefab_is_different = true;

            this.prefab = prefab;

            if (prefab_is_different)
                UpdatePreviewPrefab();
        }

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            if (!Application.isPlaying)
            {
                prev_position = position;

                if (brushTarget)
                {
                    prev_brushTarget = brushTarget;
                }

                brushTarget = prev_brushTarget;

                // Do not allow editing palettes
                if (brushTarget.layer == 31)
                    return;

                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                instance.transform.rotation = preview_go.transform.rotation;

                if (instance != null)
                {
                    Erase(grid, brushTarget, position);

                    Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
                    Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");

                    instance.transform.SetParent(brushTarget.transform);

                    instance.transform.position =
                        grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));
                }
            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            if (!Application.isPlaying)
            {
                if (brushTarget)
                {
                    prev_brushTarget = brushTarget;
                }

                brushTarget = prev_brushTarget;

                // Do not allow editing palettes
                if (brushTarget.layer == 31)
                    return;

                Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));

                if (erased != null)
                    Undo.DestroyObjectImmediate(erased.gameObject);
            }
        }

        public override void Rotate(RotationDirection direction, GridLayout.CellLayout layout)
        {
            if (tool_active)
            {
                if (direction == RotationDirection.Clockwise)
                    PreviewPrefabRotateRight();
                else
                    PreviewPrefabRotateLeft();
            }
        }

        private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
        {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position))
                    return child;
            }
            return null;
        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset)
        {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }

        public Vector3 GridPosToWorldPos(GridLayout grid, Vector3Int position)
        {
            Vector3 ret = Vector3.zero;

            ret = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));

            return ret;
        }

        public void UpdatePreviewPrefab()
        {
            bool was_active = false;

            if (preview_go != null)
            {
                was_active = preview_go.activeSelf;

                DestroyImmediate(preview_go);
            }

            preview_go = null;

            if (prefab != null)
            {
                Quaternion rotation = prefab.transform.rotation;

                preview_go = Instantiate(prefab, new Vector3(0, 0, 0), rotation);

                preview_go.hideFlags = HideFlags.HideInHierarchy;

                preview_go.SetActive(was_active);
            }
        }

        public void PreviewPrefabSetPos(Vector3 pos)
        {
            if(preview_go != null)
            {
                preview_go.transform.position = pos;
            }
        }

        public void PreviewPrefabSetActive(bool set)
        {
            if (preview_go != null)
            {
                preview_go.SetActive(set);
            }
        }

        public void PreviewPrefabRotateLeft()
        {
            if(preview_go != null)
            {
                Vector3 curr_rotation = preview_go.transform.eulerAngles;
                preview_go.transform.eulerAngles = new Vector3(curr_rotation.x, curr_rotation.y, curr_rotation.z + 90);
            }
        }

        public void PreviewPrefabRotateRight()
        {
            if (preview_go != null)
            {
                Vector3 curr_rotation = preview_go.transform.eulerAngles;
                preview_go.transform.eulerAngles = new Vector3(curr_rotation.x, curr_rotation.y, curr_rotation.z - 90);
            }
        }

        public void SetToolActive(bool set)
        {
            tool_active = set;
        }

        public bool GetToolActive()
        {
            return tool_active;
        }
    }
}

#endif
