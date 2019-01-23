using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(PrefabBrushPainter))]
public class PrefabBrushPainterEd : GridBrushEditor
{
    public override void OnMouseEnter()
    {
        if (!Application.isPlaying)
        {
            PrefabBrushPainter myScript = (PrefabBrushPainter)target;

            if (myScript.GetToolActive())
                myScript.PreviewPrefabSetActive(true);
        }
    }

    public override void OnMouseLeave()
    {
        if (!Application.isPlaying)
        {
            PrefabBrushPainter myScript = (PrefabBrushPainter)target;

            if (myScript.GetToolActive())
                myScript.PreviewPrefabSetActive(false);
        }
    }

    public override void OnToolDeactivated(GridBrushBase.Tool tool)
    {
        if (!Application.isPlaying)
        {
            PrefabBrushPainter myScript = (PrefabBrushPainter)target;

            if (tool == GridBrushBase.Tool.Paint)
            {
                myScript.SetToolActive(false);
            }
            myScript.PreviewPrefabSetActive(false);
        }
    }

    public override void OnToolActivated(GridBrushBase.Tool tool)
    {
        if (!Application.isPlaying)
        {
            PrefabBrushPainter myScript = (PrefabBrushPainter)target;

            if (tool == GridBrushBase.Tool.Paint)
            {
                myScript.SetToolActive(true);

                myScript.PreviewPrefabSetActive(true);
            }
            else
                myScript.SetToolActive(false);
        }
    }

    public override void PaintPreview(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if (!Application.isPlaying)
        {
            PrefabBrushPainter myScript = (PrefabBrushPainter)target;

            Vector3 pos = myScript.GridPosToWorldPos(gridLayout, position);

            myScript.PreviewPrefabSetPos(pos);
        }
    }
}

#endif
