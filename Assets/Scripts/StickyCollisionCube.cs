using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;

public class StickyCollisionCube : MonoBehaviour
{
    private bool isPlaneFixed = false;
    
    private GameObject drawCollisionCube;

//    private GameObject theCube;


    // Use this for initialization
    void Start()
    {
        drawCollisionCube = GameObject.Find("MarkerCube");
    }

    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void stickPlane()
    {
//        isPlaneFixed = true;
        Debug.Log("PlaneStuck");
        transform.position = drawCollisionCube.transform.position * 1;
        Quaternion transformRotation = transform.rotation;
        transformRotation.y = drawCollisionCube.transform.rotation.y;
        transform.rotation = transformRotation;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void unstickPlane()
    {
//        isPlaneFixed = false;
        GetComponent<BoxCollider>().enabled = false;

    }
}