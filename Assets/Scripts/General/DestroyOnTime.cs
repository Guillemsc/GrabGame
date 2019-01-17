using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        timer.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(timer.ReadTime() > destroy_time)
        {
            Destroy(gameObject);
        }
	}

    [SerializeField]
    private float destroy_time = 0.0f;

    Timer timer = new Timer();
}
