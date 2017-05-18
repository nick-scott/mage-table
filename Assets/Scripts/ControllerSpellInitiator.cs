using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ControllerSpellInitiator : MonoBehaviour
    {
        private SteamVR_TrackedObject _trackedObj;

        private GameObject _spellOrb;

        private GameObject _markerCube;

        private ControllerSpellSelector _spellSelector;

        private readonly List<GameObject> _spellBucket = new List<GameObject>();

        private readonly List<GameObject> _markerBucket = new List<GameObject>();

        private Vector3 _firstMarkerVector;

        private Quaternion _firstMarkerQuaternion;

        private Vector3 _lastMarkerCheckpointVector;

        private Boolean _triggerDown;

        private Boolean _traceStarted;

        private Boolean _castSpell;

        private GameObject _debugText;

        private CubeElasticity _cubeElasticity;

        private GameObject _spellDrawingSphere;

        private readonly Queue<Vector3> _vectorQueue = new Queue<Vector3>();

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) _trackedObj.index); }
        }

        // Use this for initialization
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            print("Initialized");
            _spellOrb = GameObject.Find("SpellOrb");
            _markerCube = GameObject.Find("MarkerCube");
            _spellSelector = FindObjectOfType<ControllerSpellSelector>();
            _cubeElasticity = FindObjectOfType<CubeElasticity>();
            _spellDrawingSphere = GameObject.Find("Sphere");
            _spellDrawingSphere.GetComponent<MeshRenderer>().enabled = false;
            _debugText = GameObject.Find("DebugText");
            _debugText.GetComponent<TextMesh>().text = "Started";
        }

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            _trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        // Update is called once per frame
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            var touchpadAxis = Controller.GetAxis();
            Vector3 markerDrawerPosition = _cubeElasticity.getRealLocation();
            if (touchpadAxis != Vector2.zero)
            {
            }
            if (Controller.GetHairTrigger())
            {
                if (_triggerDown)
                {
                    const double startRadiusInMeters = 0.05;
                    const double cubeMarkerDistanceInMeters = 0.02;
                    if (!_traceStarted && Vector3.Distance(_firstMarkerVector, markerDrawerPosition) >
                        startRadiusInMeters)
                    {
                        _traceStarted = true;
                        _lastMarkerCheckpointVector = markerDrawerPosition;
                        DrawMarkerCube(markerDrawerPosition);
                    }
                    if (Vector3.Distance(_lastMarkerCheckpointVector, markerDrawerPosition) >
                        cubeMarkerDistanceInMeters && _traceStarted)
                    {
                        _lastMarkerCheckpointVector = markerDrawerPosition;
                        DrawMarkerCube(markerDrawerPosition);
                    }
                    if (Vector3.Distance(_firstMarkerVector, markerDrawerPosition) < startRadiusInMeters &&
                        _traceStarted)
                    {
                        Controller.TriggerHapticPulse(3999);
                        _triggerDown = false;
                        _castSpell = true;
                        Shape shape = ShapeIdentifier.getShape(_markerBucket);
                        Debug.Log(shape);
                        _debugText.GetComponent<TextMesh>().text = shape.ToString();
                        ClearObjectBucket(_markerBucket);
                    }
                }
            }

            if (Controller.GetHairTriggerDown())
            {
                GameObject originCube = GameObject.Find("OriginCube");
                _spellSelector = FindObjectOfType<ControllerSpellSelector>();
                _firstMarkerVector = originCube.transform.position;
                Vector3 planeRotation = Controller.transform.rot.eulerAngles;
                planeRotation.z = 0;
                planeRotation.x = 0;
                _firstMarkerQuaternion = Quaternion.Euler(planeRotation);
                _cubeElasticity.setPlane(_firstMarkerQuaternion * Vector3.forward, _firstMarkerVector);
                _spellDrawingSphere.GetComponent<MeshRenderer>().enabled = true;
                DrawMarkerCube(_firstMarkerVector);
                _triggerDown = true;
            }

            if (Controller.GetHairTriggerUp())
            {
                Debug.Log("Trigger Up");
                if (_castSpell)
                {
                    Debug.Log("Spell cast complete");
                    _spellSelector = FindObjectOfType<ControllerSpellSelector>();
                    GameObject originCube = GameObject.Find("OriginCube");
                    GameObject clonie = Instantiate(_spellOrb, originCube.transform.position, Controller.transform.rot);
                    String spell = _spellSelector != null ? _spellSelector.getCurrentSpell() : "FIRE";
                    switch (spell)
                    {
                        case "FIRE":
                            clonie.GetComponent<Renderer>().material.color = Color.red;
                            break;
                        case "WATER":
                            clonie.GetComponent<Renderer>().material.color = Color.blue;
                            break;
                        case "EARTH":
                            clonie.GetComponent<Renderer>().material.color = Color.yellow;
                            break;
                        case "AIR":
                            clonie.GetComponent<Renderer>().material.color = Color.white;
                            break;
                    }
                    clonie.GetComponent<Rigidbody>().velocity = Controller.velocity;
                    clonie.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
                    _spellBucket.Add(clonie);
                }
                _triggerDown = false;
                _traceStarted = false;
                _castSpell = false;
                _cubeElasticity.resetPlane();
                ClearObjectBucket(_markerBucket);
                _vectorQueue.Clear();
                _spellDrawingSphere.GetComponent<MeshRenderer>().enabled = false;
            }

            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                ClearObjectBucket(_spellBucket);
                ClearObjectBucket(_markerBucket);
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

        private void DrawMarkerCube(Vector3 position)
        {
            GameObject markerClone = Instantiate(_markerCube, position, _firstMarkerQuaternion);
            _markerBucket.Add(markerClone);
            Controller.TriggerHapticPulse(200);
            _vectorQueue.Enqueue(position);
            if (_vectorQueue.Count > 3)
            {
                _vectorQueue.Dequeue();
                Vector3[] vectorArray = _vectorQueue.ToArray();
                double angle = ShapeIdentifier.angleBetween3Points(vectorArray[0], vectorArray[1], vectorArray[2]);
                _debugText.GetComponent<TextMesh>().text = String.Format("Angle: {0:0.00}", angle);
            }
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