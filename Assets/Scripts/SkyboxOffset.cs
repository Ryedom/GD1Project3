using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxOffset : MonoBehaviour {
	[SerializeField]
	Material _skyboxMaterial;

	void Start () {
		#if (UNITY_IOS || UNITY_ANDROID)
		gameObject.SetActive(false);
		#endif
	}

	void Update () {
		_skyboxMaterial.SetFloat("_twinkleOffset",Time.time % (2 * Mathf.PI));
	}
}
