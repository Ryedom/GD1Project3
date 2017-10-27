using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour {

    public GameObject target;
    public float lead;
    Rigidbody targetBod;

	// Use this for initialization
	void Start () {
        targetBod = target.GetComponent<Rigidbody>();
        print(targetBod != null);
	}

	// Update is called once per frame
	void Update () {
        // transform.LookAt(target.transform.position + (targetBod != null ? targetBod.velocity * lead : Vector3.zero), Vector3.up);
        transform.rotation = Quaternion.LookRotation((targetBod != null ? (target.transform.position + targetBod.velocity * lead) : target.transform.position) - transform.position, Vector3.up);
        print(targetBod.velocity);
	}
}
