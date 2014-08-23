using UnityEngine;
using System.Collections;

public class CyberspaceControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Wireframe
	void OnPreRender() {
		GL.wireframe = true;
	}
	void OnPostRender() {
		GL.wireframe = false;
	}
}
