using UnityEngine;
using System.Collections;

public class Wireframe : MonoBehaviour {

	// Wireframe
	void OnPreRender() {
		GL.wireframe = true;
	}
	void OnPostRender() {
		GL.wireframe = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
