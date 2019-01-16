using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void Awake()
    {
        PlatformManager.Instance.AddPlatform(this);

        edge_collider = gameObject.GetComponent<EdgeCollider2D>();
        base_collider = gameObject.GetComponent<Collider2D>();
    }

    private void OnDestroy()
    {
        if(PlatformManager.Instance != null)
            PlatformManager.Instance.RemovePlatform(this);
    }

    public bool GetUsable()
    {
        return edge_collider.enabled;
    }

    public bool GetCanGoDown()
    {
        return can_go_down;
    }

    public Collider2D GetCollider()
    {
        return base_collider;
    }

    private EdgeCollider2D edge_collider = null;
    private Collider2D base_collider = null;

    [SerializeField] private bool can_go_down = true;
}
