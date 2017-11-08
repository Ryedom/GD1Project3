using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkProjectile : MonoBehaviour {
	[SerializeField]
	float _lifetime;
	float _lifeTimer;
    [SerializeField]
	float _barkSpeed;
    float _barkHeight;
	PoolObject _poolObj;
	//public float _falloffAngle;

	void Start () {
		_poolObj = GetComponent<PoolObject>();
		_poolObj.OnActivate += Activate;
		_lifeTimer = _lifetime;
	}

	void Activate() {
		_lifeTimer = _lifetime;
        RaycastHit hit = new RaycastHit();
        if(!Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            print("Can't find terrain. Make sure that at least one object is on the terrain layer");
        }
        _barkHeight = hit.distance;
	}

	void Update () {
		_lifeTimer -= Time.deltaTime;
		if (_lifeTimer < 0.0f) {
			_poolObj.Kill();
			return;
		}

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
        float yDiff = hit.distance - _barkHeight;


        transform.Translate(0, -yDiff, _barkSpeed * 60.0f * Time.deltaTime);

	}
}
