using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGame : MonoBehaviour {
	[SerializeField]
	string[] _phrases;

	// Use this for initialization
	void Start () {
		if (_phrases.Length > 0) {
			GetComponent<Text>().text = _phrases[Random.Range(0,_phrases.Length)];
		}
		else GetComponent<Text>().text = "Wow!";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			SceneManager.LoadScene ("Menu");
		}
	}
}
