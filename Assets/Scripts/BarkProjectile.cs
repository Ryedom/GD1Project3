using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkProjectile : MonoBehaviour {
	[SerializeField]
	float _lifetime;
	float _lifeTimer;

	void Start () {
		_lifeTimer = _lifetime;
		
	}

	void Update () {
		_lifeTimer -= Time.deltaTime;
		if (_lifeTimer < 0.0f) {
			GameObject.Destroy(gameObject);
		}
		transform.position += transform.rotation * Vector3.forward;
	}
}
