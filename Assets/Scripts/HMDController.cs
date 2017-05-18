using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public Quaternion getRotation()
    {
        return transform.rotation;
    }
}
