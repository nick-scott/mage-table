using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpellWand : MonoBehaviour
    {
        private SteamVR_TrackedObject _trackedObj;

        private readonly List<GameObject> _spellBucket = new List<GameObject>();

        private Vector3 _lastMarkerCheckpointVector;

        private GameObject _debugText;

        private SpellFramework spellFramework;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) _trackedObj.index); }
        }

        // Use this for initialization
        private void Start()
        {
            print("Initialized");
            _debugText = GameObject.Find("DebugText");
            _debugText.GetComponent<TextMesh>().text = "Started";
            spellFramework = SpellFramework.CreateComponent(transform.parent.gameObject, Controller);
        }

        private void Awake()
        {
            _trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        // Update is called once per frame
        private void Update()
        {
            var touchpadAxis = Controller.GetAxis();
            if (touchpadAxis != Vector2.zero)
            {
            }
            if (Controller.GetHairTrigger())
            {
            }

            if (Controller.GetHairTriggerDown())
            {
                spellFramework.startSpellTrace();
            }

            if (Controller.GetHairTriggerUp())
            {
                Debug.Log("Trigger Up");
                GameObject spell = spellFramework.stopSpellTrace();
                if (spell)
                {
                    _spellBucket.Add(spell);
                }
            }

            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                ClearObjectBucket(_spellBucket);
            }


            if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
            }
        }

        private static void ClearObjectBucket(List<GameObject> objectBucket)
        {
            foreach (GameObject cubeClone in objectBucket)
            {
                Destroy(cubeClone);
            }
            objectBucket.Clear();
        }


        public Vector3 GetRealPosition()
        {
            return Controller.transform.pos;
        }

        public Quaternion GetRealRot()
        {
            return Controller.transform.rot;
        }
    }
}