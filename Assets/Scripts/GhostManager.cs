using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehavior{
	public PlayerHealth playerHealth;
	public GameObject enemy;
	public float spawnTime = 1f;
	public Transform[] spawnPoints;

	void Start(){
		//Call Spawn function in intervals of spawnTime
		InvokeRepeating("Spawn",spawnTime,spawnTime);
	}

	void Spawn(){
		//End if player is dead
		if (playerHealth.currentHealth <= 0f) {
			return;
		}

		//Spawn an enemy somewhere
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
	}
}