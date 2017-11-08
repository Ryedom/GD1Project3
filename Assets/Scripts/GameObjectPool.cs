using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolArray<T> {
	private int _poolSize;
	public int Count {
		get {
			return _poolSize;
		}
	}
	public T[] objects;
	public PoolArray(int size) {
		_poolSize = size;
		objects = new T[size];
	}
}

public class GameObjectPool : MonoBehaviour {
	[SerializeField]
	GameObject PoolPrefab;
	[SerializeField]

	int _poolSize;
	public int PoolSize {
		get {
			return _poolSize;
		}
	}
	List<GameObject> _free;
	public int FreeCount {
		get {
			return _free.Count;
		}
	}
	List<GameObject> _used;
	public int UsedCount {
		get {
			return _used.Count;
		}
	}
	PoolArray<GameObject> _pool;

	void Awake() {
		_pool = new PoolArray<GameObject>(_poolSize);
		_free = new List<GameObject>();
		_used = new List<GameObject>();
		for (int i = 0; i < _poolSize; i++) {
			_pool.objects[i] = GameObject.Instantiate(PoolPrefab,Vector3.zero,Quaternion.identity,transform);
		}
	}

	void Start() {
		for (int i = 0; i < _poolSize; i++) {
			_free.Add(_pool.objects[i]);
			PoolObject poolObject = _pool.objects[i].GetComponent<PoolObject>();
			poolObject.SetPool(this);
			_pool.objects[i].SetActive(false);
		}
	}

	public GameObject Get() {
		if (_free.Count > 0) {
			GameObject getObj = _free[_free.Count - 1];
			_free.RemoveAt(_free.Count - 1);
			_used.Add(getObj);
			return getObj;
		}
		else return null;
	}

	public void Return(GameObject returnObj) {
		if (_used.IndexOf(returnObj) != -1) {
			_used.Remove(returnObj);
			_free.Add(returnObj);
		}
	}
}
