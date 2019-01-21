using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(!disabled_on_start);
    }

    [SerializeField]
    private bool disabled_on_start = true;
}
