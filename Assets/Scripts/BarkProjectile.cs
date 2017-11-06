using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkProjectile : MonoBehaviour {
	[SerializeField]
	float _lifetime;
	float _lifeTimer;
	public float _barkSpeed;
	//public float _falloffAngle;

	void Start () {
		_lifeTimer = _lifetime;
		
	}

	void Update () {
		_lifeTimer -= Time.deltaTime;
		if (_lifeTimer < 0.0f) {
			GameObject.Destroy(gameObject);
		}

		transform.position += transform.forward * _barkSpeed;

	}
}
