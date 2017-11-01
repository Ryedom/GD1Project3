using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour {

    public GameObject target;
    AnimationCurve ease;
    public float accelTime;
    float startTime;
    public float maxLead;
    float lead;
    float radius;
    Rigidbody targetBod;
    bool starting;

	// Use this for initialization
	void Start () {
        targetBod = target.GetComponent<Rigidbody>();
        print(targetBod != null);
        starting = false;
        lead = 0;
        ease = AnimationCurve.EaseInOut(0f, 0f, accelTime, maxLead);
	}

	// Update is called once per frame
	void Update () {
        radius = Vector3.Distance(transform.position, target.transform.position);
        // transform.LookAt(target.transform.position + (targetBod != null ? targetBod.velocity * lead : Vector3.zero), Vector3.up);
        // I'm sorry for nesting a ternary expressoin, it made sense at the time
        if(Input.GetKeyDown("w") || Input.GetKeyDown("s")) {
            starting = true;
            startTime = Time.time;
        }

        lead = Mathf.Lerp(0, maxLead, (Time.time - startTime) / accelTime) * (radius / 100);
        transform.rotation = Quaternion.LookRotation((targetBod != null ? (target.transform.position + targetBod.velocity * lead) : target.transform.position) - transform.position, Vector3.up);
        // print(targetBod.velocity);
	}
}
