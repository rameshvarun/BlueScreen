using UnityEngine;
using System.Collections;

public class RotorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * 1000.0f, Vector3.up);
	}
}
