using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SpellFramework : MonoBehaviour
{
	private bool _triggerDown;
	private bool _traceStarted;
	private Vector3 _firstMarkerVector;
	private Vector3 _lastMarkerCheckpointVector;
	private bool _castSpell;
	private List<GameObject> _markerBucket = new List<GameObject>();
	private RaycastEasel _raycastEasel;
	private GameObject _wandTip;
	private Queue<Vector3> _vectorQueue = new Queue<Vector3>();
	private SpellShield _spellSelector;
	private Quaternion _firstMarkerQuaternion;
	private static GameObject _spellDrawingSphere;
	private GameObject _spellOrb;
	private ParticleSystem _spellSpark;
	private GameObject _debugText;


	public static SpellFramework CreateComponent(GameObject where, SteamVR_Controller.Device controller)
	{
		SpellFramework framework = where.AddComponent<SpellFramework>();
		framework.Controller = controller;
		return framework;
	}

	// Use this for initialization
	void Start()
	{
		_raycastEasel = FindObjectOfType<RaycastEasel>();
		_spellOrb = GameObject.Find("SpellOrb");
		_wandTip = GameObject.Find("MarkerCube");
		_spellSpark = GameObject.Find("SpellSpark").GetComponent<ParticleSystem>();
		_spellSpark.Stop();
		_spellDrawingSphere = GameObject.Find("Sphere");
		_spellDrawingSphere.GetComponent<MeshRenderer>().enabled = false;
		_debugText = GameObject.Find("DebugText");

	}

	// Update is called once per frame
	void Update()
	{
		Vector3 markerDrawerPosition = _raycastEasel.getRealLocation();
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
				_spellDrawingSphere.GetComponent<MeshRenderer>().enabled = false;
				_spellSpark.Stop();
				ClearObjectBucket(_markerBucket);
			}
		}
	}


	private void DrawMarkerCube(Vector3 position)
	{
		GameObject markerClone = Instantiate(_wandTip, position, _firstMarkerQuaternion);
		_markerBucket.Add(markerClone);
		Controller.TriggerHapticPulse(200);
		_vectorQueue.Enqueue(position);
		if (_vectorQueue.Count > 3)
		{
			_vectorQueue.Dequeue();
//            Vector3[] vectorArray = _vectorQueue.ToArray();
//            double angle = ShapeIdentifier.angleBetween3Points(vectorArray[0], vectorArray[1], vectorArray[2]);
//            _debugText.GetComponent<TextMesh>().text = String.Format("Angle: {0:0.00}", angle);
		}
	}


	public void startSpellTrace()
	{
		GameObject originCube = GameObject.Find("OriginCube");
		_spellSelector = FindObjectOfType<SpellShield>();
		_firstMarkerVector = originCube.transform.position;
		Vector3 planeRotation = Controller.transform.rot.eulerAngles;
		planeRotation.z = 0;
		planeRotation.x = 0;
		_firstMarkerQuaternion = Quaternion.Euler(planeRotation);
		Debug.Log("First Marker: " + _firstMarkerVector +
				  "\nPlane Rotation: " + planeRotation);
		_raycastEasel.setPlane(_firstMarkerQuaternion * Vector3.forward, _firstMarkerVector);
		_spellDrawingSphere.GetComponent<MeshRenderer>().enabled = true;
		_spellSpark.Play();

		DrawMarkerCube(_firstMarkerVector);
		_triggerDown = true;
		Debug.Log("Spell Trace Started");
	}

	public GameObject stopSpellTrace()
	{
		GameObject clonie = null;
		if (_castSpell)
		{
			Debug.Log("Spell cast complete");
			_spellSelector = FindObjectOfType<SpellShield>();
			GameObject originCube = GameObject.Find("OriginCube");
			clonie = Instantiate(_spellOrb, originCube.transform.position, Controller.transform.rot);
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
		}
		_triggerDown = false;
		_traceStarted = false;
		_castSpell = false;
		_raycastEasel.resetPlane();
		ClearObjectBucket(_markerBucket);
		_vectorQueue.Clear();
		_spellDrawingSphere.GetComponent<MeshRenderer>().enabled = false;
		_spellSpark.Stop();
		_spellSpark.Clear();
		return clonie;
	}

	private static void ClearObjectBucket(List<GameObject> objectBucket)
	{
		foreach (GameObject cubeClone in objectBucket)
		{
			Destroy(cubeClone);
		}
		objectBucket.Clear();
	}

	public SteamVR_Controller.Device Controller { get; set; }
}