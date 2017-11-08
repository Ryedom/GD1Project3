using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {
	GameObjectPool _sourcePool;
	public delegate void PoolCallback();
	public event PoolCallback OnActivate = delegate { };
	public event PoolCallback OnKill = delegate { };

	public void SetPool(GameObjectPool newSource) {
		if (_sourcePool == null)
			_sourcePool = newSource;
	}

	public void Activate() {
		gameObject.SetActive(true);
		OnActivate();
	}
	
	public void Kill() {
		OnKill();
		gameObject.SetActive(false);
		_sourcePool.Return(gameObject);
	}
}
