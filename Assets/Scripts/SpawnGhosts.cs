using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGhosts : MonoBehaviour {
	[SerializeField]
	GameObject _ghostPrefab;
	float _spawnRate = 1f;
	float _spawnRadius = 100f;
	int _spawnPointCount = 10;
	Vector3[] _spawnPoints;


	// Use this for initialization
	void Start () {
		Vector3 center = transform.position;
		float angle = (360f / _spawnPointCount) * Mathf.Deg2Rad;

		_spawnPoints = new Vector3[_spawnPointCount];
		for(int i = 0; i < _spawnPointCount; i++){
			Vector3 newPos = Vector3.zero;
			newPos.x = center.x + _spawnRadius * Mathf.Cos(angle*i);
			newPos.y = center.y;
			newPos.z = center.z + _spawnRadius * Mathf.Sin(angle*i);
			_spawnPoints[i] = newPos;
		}
		print("Calling Invoke");
		InvokeRepeating("Spawn", _spawnRate, _spawnRate);
	}
	
	// Update is called once spawnrate
	void Spawn() {
		int i = Random.Range(0, _spawnPointCount);
		Vector3 ghost_pos = _spawnPoints[i];
		Quaternion ghost_rotation = Quaternion.LookRotation(transform.position - ghost_pos);
		Instantiate(_ghostPrefab, ghost_pos, ghost_rotation, transform);
	}

	void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(transform.position,_spawnRadius);
	}
}
