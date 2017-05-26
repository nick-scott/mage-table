using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class RaycastEasel : MonoBehaviour
{

    private Plane plane;

    private Vector3 position = new Vector3();

    // Use this for initialization
    void Start()
    {
        resetPlane();
    }

    // Update is called once per frame
    void Update()
    {
        Transform parent = transform.parent;
        Ray ray = new Ray(parent.transform.position, parent.transform.rotation * Vector3.forward);
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            position = ray.GetPoint(rayDistance);
            transform.position = position;
        }
    }

    public void setPlane(Vector3 inNormal, Vector3 inPoint)
    {
        plane = new Plane(inNormal, inPoint);
    }

    public void setNormalAndPoint(Vector3 inNormal, Vector3 inPoint)
    {
        plane.SetNormalAndPosition(inNormal, inPoint);
    }

    public void resetPlane()
    {
        plane = new Plane(new Vector3(1, 1, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, 1));
    }

    public Vector3 getRealLocation()
    {
        return position;
    }

    public Plane getPlane()
    {
        return plane;
    }
}