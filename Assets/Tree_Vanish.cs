using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Vanish : MonoBehaviour {

	public GameObject camera;
	public GameObject dog;
	public float transparency_level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Renderer tree_rend = GetComponent<Renderer> ();

		if (tree_rend.isVisible) {
			for (int i = 0; i < tree_rend.materials.Length; i++) { 
				float r = tree_rend.materials [i].color.r;
				float g = tree_rend.materials [i].color.g;
				float b = tree_rend.materials [i].color.b;
				tree_rend.materials[i].shader = Shader.Find("Transparent/Diffuse");
				Vector4 c = new Vector4(r, g, b, transparency_level);
				tree_rend.materials[i].SetColor ("_Color", c);
			}

		}
	}
}
