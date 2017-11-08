using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	[SerializeField]
	Transform _follow;
	[SerializeField]
	Camera _thisCamera;
	Vector3 _startPosition;
	Vector3 _currentPosition;
	Quaternion _currentRotation;

	// Use this for initialization
	void Start () {
		if (!_thisCamera)
			_thisCamera = GetComponent<Camera>();
		_startPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = transform.position;
		Vector3 playerDirection = (_follow.position - _startPosition);
		pos.y = Mathf.MoveTowards(pos.y,_startPosition.y + playerDirection.magnitude,10.0f);
		playerDirection = (_follow.position - transform.position);
		_currentRotation = Quaternion.LookRotation(playerDirection.normalized);
		_currentPosition = pos;
	}

	void LateUpdate() {
		transform.rotation = _currentRotation;
		transform.position = _currentPosition;
	}
}
