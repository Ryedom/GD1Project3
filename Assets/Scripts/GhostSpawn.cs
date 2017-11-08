using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GhostSpawn : MonoBehaviour {
	public GameObject ghost;
	[SerializeField]
	GameObjectPool _ghostPool;
	[SerializeField]
	GameObjectPool _bigGhostPool;
	public float spawnrate = 5.0f;
	public float radius = 100;
	public int spawnpoint_count = 10;
	public float waveInitTime = 99.0f;
	public GameObject timer;
	public GameObject wave_note;
	List<Vector3> spawnpoints = new List<Vector3>();
	float timeLeft;
	int level = 1;
	int levelStage = 1;
	int remaining_ghosts = 0;

	// Use this for initialization
	void Start () {

		//calculate the circle that they spawn from
		Vector3 center = transform.position;
		float angle = (360 / spawnpoint_count) * Mathf.Deg2Rad;

		for(int i = 1; i <= spawnpoint_count; i++){
			float x = radius * Mathf.Cos(angle*i) + center.x;
			float y = center.y;
			float z = radius * Mathf.Sin(angle*i) + center.z;
			RaycastHit hit = new RaycastHit();
	        Physics.Raycast(new Ray(new Vector3(x, y, z), Vector3.down), out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
			y -= (hit.distance - 1);
			Vector3 temp = new Vector3 (x, y, z);
			spawnpoints.Add(temp);
		}

		timeLeft = waveInitTime;
		//print ("Calling Invoke");
		InvokeRepeating ("Spawn", spawnrate, spawnrate);

	}

	//Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		Text timertext = timer.gameObject.GetComponent<Text> ();
		timertext.text = "Time Left: " + Mathf.Max(Mathf.Round(timeLeft),0);
		Text wavetext = wave_note.gameObject.GetComponent<Text> ();
		wavetext.text = "Wave " + level;

		//bool check = true;
		//make the level difficulty varied
		if (timeLeft > 80) {
			levelStage = 1;
		} else if (80 > timeLeft && timeLeft > 40) {
			levelStage = 2;
		} else if (timeLeft < 40) {
			levelStage = 3;
		}

		if (levelStage == 1) {
			spawnrate -= .0005f;
		} else if (levelStage == 3) {
			spawnrate += .0005f;
		}

		//if the timer ends, start the next phase, or end the game
		if (timeLeft <= 0) {
			CancelInvoke ("Spawn");
			if (level == 3) {
				if (GameObject.FindGameObjectsWithTag("Ghost").Length == 0) {
					SceneManager.LoadScene("WinState");
				}
			} else {
				level += 1;
				timeLeft = waveInitTime;
				spawnrate -= 1;
				InvokeRepeating("Spawn", spawnrate, spawnrate);
			}
		}

	}


	// Spawn function
	void Spawn () {
		int i = Random.Range (0, spawnpoint_count);
		Vector3 ghost_pos = spawnpoints [i];
		Quaternion ghost_rotation = Quaternion.LookRotation(transform.position - ghost_pos);
		GameObject newGhost;
		if (Random.value < 0.1f && level == 3)
			newGhost = _bigGhostPool.Get();
		else
			newGhost = _ghostPool.Get(); //Instantiate (ghost, ghost_pos, ghost_rotation);
		if (newGhost != null) {
			newGhost.transform.position = ghost_pos;
			newGhost.transform.rotation = ghost_rotation;
			newGhost.GetComponent<PoolObject>().Activate();
		}
	}


	//Check lose condition
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Ghost") {
			print ("You Lost!");
			SceneManager.LoadScene ("GameOver");
		}
	}
}
