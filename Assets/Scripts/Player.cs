using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField]
	GameObject _barkPrefab;
	[SerializeField]
	GameObjectPool _barkPool;
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
	[SerializeField]
	AudioClip[] _barkSounds;

	Rigidbody _rigid;
	Transform _model;
    Animator _dogAnimator;
	RaycastHit _normalHit;
	Vector3 _normal = Vector3.up;
	Vector2 _inputVector;

	Quaternion _rotationTurn = Quaternion.identity; // Rotation of the player turning
	Quaternion _rotationPlane = Quaternion.identity; // Rotation of the player snapping to the ground
	bool _canBark = true;
	float _barkTimer;
	AudioSource _audioSource;

	void Start () {
		_rigid = GetComponent<Rigidbody> ();
        // _model = transform.GetChild(0);
        _dogAnimator = _dog.GetComponent<Animator>();
        _barkTimer = _barkLength;
		_audioSource = GetComponent<AudioSource>();
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
		if (Input.GetKey(KeyCode.R))
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);

		_inputVector = new Vector2(-Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

		// Bark timer
		if (!_canBark) {
			_barkTimer -= Time.deltaTime;
			if (_barkTimer < 0.0f)
				_canBark = true;
		}

		// Bark if pressed
		if (Input.GetButtonDown("Jump") && _canBark) {
			PoolObject newBullet = _barkPool.Get().GetComponent<PoolObject>();
			newBullet.transform.position = transform.position + transform.rotation * _barkOffset;
			newBullet.transform.rotation = transform.rotation;
			newBullet.Activate();
			//GameObject.Instantiate(_barkPrefab,transform.position + _barkOffset,transform.rotation);
			_canBark = false;
			_barkTimer = _barkLength;

			//play a random bark sound
			_audioSource.PlayOneShot(_barkSounds[Random.Range(0, _barkSounds.Length)], 0.5f);
		}
	
		//var dog_controller = dog_animator.GetComponent<AnimatorControllerParameter> ();
		// Keep track of the current normal (by default it's world up)

		// Find the closest normal to the ground
		if (Physics.Raycast(transform.position,-_normal,out _normalHit,5.0f,_playerCastMask)) {
			_normal = _normalHit.normal;
		}
		else _normal = Vector3.up;

//		Vector2 rawInputVector = inputVector;
		_inputVector.Normalize();
		float inputAngle = (Vector2.SignedAngle(Vector2.up,_inputVector) + 360.0f) % 360.0f;
		// Movement logic
		if (_inputVector.magnitude >= 0.01f) {

			if (_dog.activeSelf) {
				// print ("walking");
				if(_dogAnimator.runtimeAnimatorController != null)
					_dogAnimator.SetBool ("Walking", true);
			}

			float camAngle = Camera.main.transform.eulerAngles.y;
//			float camInputAngle = (camAngle - inputAngle + 360.0f) % 360.0f;
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
			_inputVector = Vector2.zero;
			_dogAnimator.SetBool ("Walking", false);
		}

		// Make the player be tangent to the ground below
		Vector3 right = transform.right;
		Vector3 up = _normal;
		Vector3 forward = transform.forward;

		_rotationPlane = Tools.SnapTangents(ref right,ref up,ref forward);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,_rotationPlane,10.0f * 60.0f * Time.deltaTime);

		// "Gravity" (move towards the hill)
		_rigid.AddForce(-_normal * 7.5f,ForceMode.Acceleration);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(transform.position,transform.forward * 5.0f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(transform.position,transform.right * 5.0f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(transform.position,-transform.up * 5.0f);
	}
}
