using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehavior {
	public float Speed = 50;
	private _ghostTransform _cameraTransform;
	private Transform _ghostTransform;

	void Start(){
		//Find Camera object via text tag
		var cam = GameObject.FindGameObjectWithTag("MainCamera");
		if (!cam) {
			Debug.LogError ("Could not find camera. Check tag.");
		} else {
			_cameraTransform = _cameraTransform.transform;
		}
		_ghostTransform = this._ghostTransform;
	}

	void Update(){
		_ghostTransform.position = Vector3.MoveTowards (_ghostTransform.position,
			_cameraTransform.position, Speed * TimeSpan.deltaTime);
	}
}


