using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankControlsPLACEHOLDER : MonoBehaviour {

	public float speed;
	public float turnSpeed;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey("w"))
			// transform.Translate(0, 0, speed * Time.deltaTime);
			rb.velocity = new Vector3(speed * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad), 0, speed * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad));
		else if(Input.GetKey("s"))
			// transform.Translate(0, 0, -0.5f * speed * Time.deltaTime);
			rb.velocity = new Vector3(-0.5f * speed * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad), 0, -0.5f * speed * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad));
		else
			rb.velocity = Vector3.zero;
		if(Input.GetKey("a"))
			transform.Rotate(0, -1 * turnSpeed * Time.deltaTime, 0);
		if(Input.GetKey("d"))
			transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
	}
}
