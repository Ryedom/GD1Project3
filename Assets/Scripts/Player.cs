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
	float _playerSpeed = 5.0f;
	Rigidbody _rigid;
	Transform _model;
	RaycastHit _normalHit;
	Vector2 inputVector;

	Quaternion _rotationTurn = Quaternion.identity; // Rotation of the player turning
	Quaternion _rotationPlane = Quaternion.identity; // Rotation of the player snapping to the ground
	bool _canBark = true;
	float _barkTimer;

	void Start () {
		_rigid = GetComponent<Rigidbody> ();
		// _model = transform.GetChild(0);
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
			GameObject.Instantiate(_barkPrefab,transform.position,transform.rotation);
			_canBark = false;
			_barkTimer = _barkLength;
		}
	}

	void FixedUpdate () {
		Animator dog_animator = _dog.GetComponent<Animator> ();
		//var dog_controller = dog_animator.GetComponent<AnimatorControllerParameter> ();
		// Keep track of the current normal (by default it's world up)
		Vector3 currentNormal = Vector3.up;
		// Find the closest normal to the ground
		if (Physics.Raycast(transform.position,-transform.position.normalized,out _normalHit,1.5f,LayerMask.NameToLayer("Player"))) {
			if (Vector3.Dot(transform.position.normalized,_normalHit.normal) > 0.5f)
				currentNormal = _normalHit.normal;
		}

		Vector2 rawInputVector = inputVector;
		inputVector.Normalize();
		float inputAngle = (Vector2.SignedAngle(Vector2.up,inputVector) + 360.0f) % 360.0f;
		// Movement logic
		if (inputVector.magnitude >= 0.01f) {

			if (_dog.activeSelf) {
				//print ("walking");
				if(dog_animator.runtimeAnimatorController != null)
					dog_animator.SetBool ("Walking", true);
			}

			inputAngle += 90.0f;
			float camAngle = Camera.main.transform.eulerAngles.y;
			float camInputAngle = (camAngle - inputAngle + 360.0f) % 360.0f;
			Quaternion camRotation = Quaternion.AngleAxis(camAngle,Vector3.up);
			Quaternion inputRotation = Quaternion.AngleAxis(inputAngle,Vector3.up);

			Vector3 inputRight = transform.right;
			Vector3 inputUp = Vector3.up;
			Vector3 inputForward = transform.forward;

			Quaternion playerFacing = SnapTangents(ref inputRight, ref inputUp, ref inputForward);
			// Flip the facing vector if going more than 90 degrees around (avoid that if possible though)
			if (Vector3.Dot(Vector3.up,currentNormal) < 0.0f)
				playerFacing = Quaternion.Inverse(playerFacing);

			_rotationTurn = camRotation * inputRotation * Quaternion.Inverse(playerFacing);
			transform.rotation = Quaternion.RotateTowards(transform.rotation,transform.rotation * _rotationTurn, 10.0f);
			//transform.rotation *= _rotationTurn;
		}
		else {
			inputVector = Vector2.zero;
			if (_dog.activeSelf) {
				//print ("stop walking");
				if(dog_animator.runtimeAnimatorController != null)
					dog_animator.SetBool ("Walking", false);
			}
		}

		// Make the player be tangent to the ground below
		Vector3 right = transform.right;
		Vector3 up = currentNormal;
		Vector3 forward = transform.forward;

		_rotationPlane = SnapTangents(ref right,ref up,ref forward);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,_rotationPlane,10.0f * 60.0f * Time.deltaTime);

		// Make the player move forward now that they are tangent to the ground
		if (inputVector.magnitude > 0.1f) {
			//_rigid.AddForce(Mathf.Max(7.5f - _rigid.velocity.magnitude, 0.0f) * transform.forward, ForceMode.VelocityChange);
		}

		// "Gravity" (move towards the hill)
		_rigid.AddForce(-currentNormal * (20.0f * 60.0f) * Time.deltaTime,ForceMode.Acceleration);
	}

	Quaternion SnapTangents(ref Vector3 Right, ref Vector3 Up, ref Vector3 Forward) {
		// Orthonormalize the normal with the player's direction and return the rotation.
		Vector3.OrthoNormalize(ref Up, ref Forward, ref Right);
		Matrix4x4 rotationMatrix = Matrix4x4.identity;
		rotationMatrix.SetColumn(0,Right);
		rotationMatrix.SetColumn(1,Up);
		rotationMatrix.SetColumn(2,Forward);
		return QuaternionFromMatrix(rotationMatrix);//rotationMatrix.rotation;
	}

	Vector3 TransformVector(Quaternion quat, Vector3 vect) {
		// Transform a vector (possibly equivalent to just quat * vector in Unity, should test this)
		Quaternion vectQuat = Quaternion.identity;
		vectQuat.x = vect.x;
		vectQuat.y = vect.y;
		vectQuat.z = vect.z;
		vectQuat.w = 0.0f;
		Quaternion result = quat * vectQuat * Quaternion.Inverse(quat);
		return new Vector3(result.x,result.y,result.z);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(transform.position,transform.forward * 5.0f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(transform.position,TransformVector(_rotationTurn,Vector3.forward) * 5.0f);
	}

	// Create a quaternion from a matrix. Acts more stable than doing "mat.rotation".
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}
}
