using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	Rigidbody _rigid;
	public GameObject _follow;
	RaycastHit _normalHit;
	Vector3 _normal = Vector3.up;
	[SerializeField]
	LayerMask _ghostCastMask;
	Quaternion _rotationPlane = Quaternion.identity;
	Quaternion _rotationTurn = Quaternion.identity;

	void Start() {
		_rigid = GetComponent<Rigidbody>();
		//_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update() {

	}

	void FixedUpdate() {
		//MOVEMENT CODE
		// Find the closest normal to the ground (if possible)
		if (Physics.Raycast(transform.position,-_normal,out _normalHit,1000.0f,_ghostCastMask)) {
			_normal = _normalHit.normal;
		}
		else _normal = Vector3.up;

		Vector3 followDirection = _follow.transform.position - transform.position;
		followDirection.y = 0;
		followDirection.Normalize();
		Quaternion followFacing = Quaternion.LookRotation(followDirection);

		Vector3 currentRight = transform.right;
		Vector3 currentUp = Vector3.up;
		Vector3 currentForward = transform.forward;
		Quaternion ghostFacing = Tools.SnapTangents(ref currentRight, ref currentUp, ref currentForward);

		// Flip the facing direction if going more than 90 degrees around (avoid that if possible though)
		if (Vector3.Dot(Vector3.up,_normal) < 0.0f)
			ghostFacing = Quaternion.Inverse(ghostFacing);

		_rotationTurn = followFacing * Quaternion.Inverse(ghostFacing);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,transform.rotation * _rotationTurn, 10.0f);

		// Make the ghost tangent to the ground below
		Vector3 right = transform.right;
		Vector3 up = _normal;
		Vector3 forward = transform.forward;

		_rotationPlane = Tools.SnapTangents(ref right,ref up,ref forward);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,_rotationPlane,10.0f * 60.0f * Time.deltaTime);

		_rigid.AddForce(Mathf.Max(2.5f - _rigid.velocity.magnitude, 0.0f) * transform.forward, ForceMode.VelocityChange);

		// "Gravity" (move towards the hill)
		_rigid.AddForce(-_normal * 20.0f,ForceMode.Acceleration);
		//END MOVEMENT CODE

	}
	void OnTriggerEnter (Collider c) {
		//print ("Hello?");
		if (c.gameObject.tag == "Bullet") {
			//print ("Collision!");
			Destroy(c.gameObject);
			Destroy(gameObject);
		}
	}
}
