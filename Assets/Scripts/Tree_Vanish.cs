using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Vanish : MonoBehaviour {
	[SerializeField]
	float _transparencyLevel = 0.5f;
	
	// Update is called once per frame
	void Start () {
		Renderer tree_rend = GetComponent<Renderer> ();
		for (int i = 0; i < tree_rend.materials.Length; i++) { 
			float r = tree_rend.materials [i].color.r;
			float g = tree_rend.materials [i].color.g;
			float b = tree_rend.materials [i].color.b;
			tree_rend.materials[i].shader = Shader.Find("Transparent/Diffuse");
			Vector4 c = new Vector4(r, g, b, _transparencyLevel);
			tree_rend.materials[i].SetColor ("_Color", c);
		}
	}
}
