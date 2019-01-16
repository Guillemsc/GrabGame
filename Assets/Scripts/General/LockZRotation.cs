using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZRotation : MonoBehaviour
{
	void Update ()
    {
        if(enabled)
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
	}

    [SerializeField]
    private bool enabled = true;
}
