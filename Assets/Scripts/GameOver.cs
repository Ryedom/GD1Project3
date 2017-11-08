using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
		if (Input.GetButton("Fire")) {
			SceneManager.LoadScene ("Menu");
		}
	}
}
