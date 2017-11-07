using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientMinimap : MonoBehaviour {
	Quaternion _root;
	[SerializeField]
	Transform _follow;

	void Awake () {
		_root = transform.rotation;
	}
	
	void LateUpdate () {
		Vector3 followDirection = _follow.position - transform.position;
		Vector2 followXZ = new Vector2(followDirection.x,followDirection.z).normalized;
		float followAngle = (Vector2.SignedAngle(Vector2.up,followXZ) + 360.0f) % 360.0f;
		transform.rotation = Quaternion.AngleAxis(-followAngle,Vector3.up) * _root;
	}
}
