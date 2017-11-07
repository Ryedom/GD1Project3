using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField]
	GameObject _barkPrefab;
	[SerializeField]
	GameObject _dog;
	[SerializeField]
	float _barkLength;
	[SerializeField]
	Vector3 _barkOffset;
	[SerializeField]
	float _playerSpeed = 5.0f;
	[SerializeField]
	LayerMask _playerCastMask;
	Rigidbody _rigid;
	Transform _model;
    Animator dog_animator;
	RaycastHit _normalHit;
	Vector3 _normal = Vector3.up;
	Vector2 inputVector;

	Quaternion _rotationTurn = Quaternion.identity; // Rotation of the player turning
	Quaternion _rotationPlane = Quaternion.identity; // Rotation of the player snapping to the ground
	bool _canBark = true;
	float _barkTimer;

	void Start () {
		_rigid = GetComponent<Rigidbody> ();
        // _model = transform.GetChild(0);
        dog_animator = _dog.GetComponent<Animator>();
        _barkTimer = _barkLength;
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
		if (Input.GetKey(KeyCode.R))
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);

		inputVector = new Vector2(-Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

		// Bark timer
		if (!_canBark) {
			_barkTimer -= Time.deltaTime;
			if (_barkTimer < 0.0f)
				_canBark = true;
		}

		// Bark if pressed
		if (Input.GetButtonDown("Jump") && _canBark) {
			GameObject.Instantiate(_barkPrefab,transform.position + _barkOffset,transform.rotation);
			_canBark = false;
			_barkTimer = _barkLength;
		}
	
		//var dog_controller = dog_animator.GetComponent<AnimatorControllerParameter> ();
		// Keep track of the current normal (by default it's world up)

		// Find the closest normal to the ground
		if (Physics.Raycast(transform.position,-_normal,out _normalHit,2.5f,_playerCastMask)) {
			if (Vector3.Dot(transform.up,_normalHit.normal) > 0.5f)
				_normal = _normalHit.normal;
		}
		else _normal = Vector3.up;

//		Vector2 rawInputVector = inputVector;
		inputVector.Normalize();
		float inputAngle = (Vector2.SignedAngle(Vector2.up,inputVector) + 360.0f) % 360.0f;
		// Movement logic
		if (inputVector.magnitude >= 0.01f) {

			if (_dog.activeSelf) {
				// print ("walking");
				if(dog_animator.runtimeAnimatorController != null)
					dog_animator.SetBool ("Walking", true);
			}

			float camAngle = Camera.main.transform.eulerAngles.y;
			float camInputAngle = (camAngle - inputAngle + 360.0f) % 360.0f;
			Quaternion camRotation = Quaternion.AngleAxis(camAngle,Vector3.up);
			Quaternion inputRotation = Quaternion.AngleAxis(inputAngle,Vector3.up);

			Vector3 inputRight = transform.right;
			Vector3 inputUp = Vector3.up;
			Vector3 inputForward = transform.forward;

			Quaternion playerFacing = Tools.SnapTangents(ref inputRight, ref inputUp, ref inputForward);
			// Flip the facing vector if going more than 90 degrees around (avoid that if possible though)
			if (Vector3.Dot(Vector3.up,_normal) < 0.0f)
				playerFacing = Quaternion.Inverse(playerFacing);

			_rotationTurn = camRotation * inputRotation * Quaternion.Inverse(playerFacing);
			transform.rotation = Quaternion.RotateTowards(transform.rotation,transform.rotation * _rotationTurn, 10.0f);
			//transform.rotation *= _rotationTurn;
		}
		else {
			inputVector = Vector2.zero;
			dog_animator.SetBool ("Walking", false);
		}

		// Make the player be tangent to the ground below
		Vector3 right = transform.right;
		Vector3 up = _normal;
		Vector3 forward = transform.forward;

		_rotationPlane = Tools.SnapTangents(ref right,ref up,ref forward);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,_rotationPlane,10.0f * 60.0f * Time.deltaTime);

		// Make the player move forward now that they are tangent to the ground
		if (inputVector.magnitude > 0.1f) {
			//_rigid.AddForce(Mathf.Max(7.5f - _rigid.velocity.magnitude, 0.0f) * transform.forward, ForceMode.VelocityChange);
		}

		// "Gravity" (move towards the hill)
		_rigid.AddForce(-_normal * 20.0f,ForceMode.Acceleration);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(transform.position,transform.forward * 5.0f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(transform.position,(_rotationTurn * Vector3.forward) * 5.0f);
	}
}
