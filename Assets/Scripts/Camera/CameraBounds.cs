using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private void Awake()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();

        if (collider != null)
            collider.isTrigger = true;
    }

    public float GetTopPoint()
    {
        float ret = 0.0f;

        if (collider != null)
        {
            Vector2 size = collider.size;
            Vector3 centerPoint = new Vector3(collider.offset.x, collider.offset.y, 0f);
            Vector3 worldPos = gameObject.transform.position + (Vector3)collider.offset;

            ret = worldPos.y + (size.y / 2f);
        }

        return ret;
    }

    public float GetBottomPoint()
    {
        float ret = 0.0f;

        if (collider != null)
        {
            Vector2 size = collider.size;
            Vector3 centerPoint = new Vector3(collider.offset.x, collider.offset.y, 0f);
            Vector3 worldPos = gameObject.transform.position + (Vector3)collider.offset;

            ret = worldPos.y - (size.y / 2f);
        }

        return ret;
    }

    public float GetLeftPoint()
    {
        float ret = 0.0f;

        if (collider != null)
        {
            Vector2 size = collider.size;
            Vector3 centerPoint = new Vector3(collider.offset.x, collider.offset.y, 0f);
            Vector3 worldPos = gameObject.transform.position + (Vector3)collider.offset;

            ret = worldPos.x - (size.x / 2f);
        }

        return ret;
    }

    public float GetRightPoint()
    {
        float ret = 0.0f;

        if (collider != null)
        {
            Vector2 size = collider.size;
            Vector3 centerPoint = new Vector3(collider.offset.x, collider.offset.y, 0f);
            Vector3 worldPos = gameObject.transform.position + (Vector3)collider.offset;

            ret = worldPos.x + (size.x / 2f);
        }

        return ret;
    }

    private BoxCollider2D collider = null;
}
