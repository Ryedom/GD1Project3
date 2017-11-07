using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostSpawn : MonoBehaviour {

	public GameObject ghost;
	public float spawnrate = 1;
	public float radius = 100;
	public int spawnpoint_count = 10;
	List<Vector3> spawnpoints = new List<Vector3>();


	// Use this for initialization
	void Start () {
		Vector3 center = transform.position;
		float angle = (360 / spawnpoint_count) * Mathf.Deg2Rad;

		for(int i = 1; i <= spawnpoint_count; i++){
			float x = radius * Mathf.Cos(angle*i) + center.x;
			float y = center.y;
			float z = radius * Mathf.Sin(angle*i) + center.z;
			Vector3 temp = new Vector3 (x, y, z);
			spawnpoints.Add(temp);
		}
		//print ("Calling Invoke");
		InvokeRepeating ("Spawn", spawnrate, spawnrate);

	}
	
	// Update is called once spawnrate
	void Spawn () {

		int i = Random.Range (0, spawnpoint_count);
		Vector3 ghost_pos = spawnpoints [i];
		Quaternion ghost_rotation = Quaternion.LookRotation(transform.position- ghost_pos);
		GameObject newGhost = Instantiate (ghost, ghost_pos, ghost_rotation);
		newGhost.GetComponent<Ghost>()._follow = transform;
	}

	void OnTriggerEnter(Collider other) {

		//if (c.collider.gameObject.tag != "Terrain") {
			//print (c.collider.gameObject.tag);
		//}

		if (other.gameObject.tag == "Ghost") {
			print ("You Lost!");
			SceneManager.LoadScene ("GameOver");
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position,radius);
	}
}

