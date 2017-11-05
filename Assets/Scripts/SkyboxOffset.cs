using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxOffset : MonoBehaviour {
	[SerializeField]
	Material _skyboxMaterial;

	void Update () {
		_skyboxMaterial.SetFloat("_twinkleOffset",Time.time % (2 * Mathf.PI));
	}
}
