using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZRotation : MonoBehaviour
{
	void Update ()
    {
        if(lock_enabled)
            gameObject.transform.eulerAngles 
                = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
	}

    [SerializeField]
    private bool lock_enabled = true;
}
