using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Valve.VR;
using Object = UnityEngine.Object;

// ReSharper disable UnusedMember.Local
// ReSharper disable SuggestVarOrType_SimpleTypes

public class ViveControllerInputTest : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;

    private GameObject cubie;

    List<Object> cubeBucket = new List<Object>();

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }

    // Use this for initialization
    private void Start()
    {
        print("Initialized");
        cubie = GameObject.Find("Cube");
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        var touchpadAxis = Controller.GetAxis();
        if (touchpadAxis != Vector2.zero)
        {
            Debug.Log(gameObject.name + touchpadAxis);
            if (isTopZone())
            {
                Debug.Log(gameObject.name + " Top Zone");
                Controller.TriggerHapticPulse(3999, EVRButtonId.k_EButton_SteamVR_Touchpad);
            }

            else if (isBottomZone())
            {
                Controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_ApplicationMenu);
            }

            else if (isLeftZone())
            {
                Debug.Log(gameObject.name + " Left Zone");
                Controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_A);
            }

            else if (isRightZone())
            {
                Debug.Log(gameObject.name + " Right Zone");
                Controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_System);
            }
        }

        if (Controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
            Debug.Log(gameObject.name + " Axis 1");
        }

        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
            GameObject clonie = Object.Instantiate(cubie, Controller.transform.pos, Controller.transform.rot);
            clonie.GetComponent<Rigidbody>().velocity = Controller.velocity;
            clonie.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
            cubeBucket.Add(clonie);
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
            foreach (Object cubeClone in cubeBucket)
            {
                GameObject.Destroy(cubeClone);
            }
        }


        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
    }

    private Boolean isTopZone()
    {
        var touchpadAxis = Controller.GetAxis();
        float x = touchpadAxis.x;
        float y = touchpadAxis.y;
        return x > -0.5 && x < 0.5 && y > 0.5;
    }

    private Boolean isLeftZone()
    {
        var touchpadAxis = Controller.GetAxis();
        float x = touchpadAxis.x;
        float y = touchpadAxis.y;
        return x < -0.5 && y > -0.5 && y < 0.5;
    }

    private Boolean isRightZone()
    {
        var touchpadAxis = Controller.GetAxis();
        float x = touchpadAxis.x;
        float y = touchpadAxis.y;
        return x > 0.5 && y > -0.5 && y < 0.5;
    }

    private Boolean isBottomZone()
    {
        var touchpadAxis = Controller.GetAxis();
        float x = touchpadAxis.x;
        float y = touchpadAxis.y;
        return x > -0.5 && x < 0.5 && y < -0.5;
    }
}