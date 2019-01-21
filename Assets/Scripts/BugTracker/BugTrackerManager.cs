using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class BugTrackerManager : Singleton<BugTrackerManager>
{
    BugTrackerManager()
    {
        InitInstance(this);
    }

	// Use this for initialization
	void Start ()
    {
        string url = "curl -u guillemsc https://api.github.com";

        WebRequest request = HttpWebRequest.Create(url);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
