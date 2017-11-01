using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour {

    [SerializeField]
    bool multiView;
    [SerializeField]
    GameObject target;
    GameObject camera;
    [SerializeField]
    float accelTime;
    [SerializeField]
    float cameraOffset;
    [SerializeField]
    float cameraOverlap;
    float startTime;
    [SerializeField]
    float maxLead;
    float lead;
    float radius;
    Rigidbody targetBod;


	// Use this for initialization
	void Start () {
        targetBod = target.GetComponent<Rigidbody>();
        lead = 0;
        camera = GameObject.Find("Main Camera");
	}

	// Update is called once per frame
	public void Update () {
        radius = Vector3.Distance(transform.position, target.transform.position);
        if(targetBod.velocity == Vector3.zero) {
            startTime = Time.time;
        }

        lead = Mathf.SmoothStep(0, maxLead, (Time.time - startTime) / accelTime) * (radius / 100);
        transform.rotation = Quaternion.LookRotation((targetBod != null ? (target.transform.position + targetBod.velocity * lead) : target.transform.position) - transform.position, Vector3.up);
        if(!multiView || radius < cameraOffset * transform.lossyScale.x) {
            camera.transform.localPosition = new Vector3(0f, 0f, -cameraOffset + cameraOverlap);
        }
        else {
            camera.transform.localPosition = new Vector3(0f, 0f, cameraOffset - cameraOverlap);
        }
	}
}
