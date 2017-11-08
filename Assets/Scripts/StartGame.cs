using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {
	[SerializeField]
	string[] _randomTitles;

	// Use this for initialization
	void Start () {
		if (_randomTitles.Length > 0) {
			GetComponent<Text>().text = _randomTitles[Random.Range(0,_randomTitles.Length)];
		}
		else GetComponent<Text>().text = "No Picnic";
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Space)) {
			SceneManager.LoadScene ("Master");
		}
	}
}
