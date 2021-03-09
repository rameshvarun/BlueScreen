using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	bool hit = false;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Hit(float power) {
		if(hit == false) {
			hit = true;
			GetComponent<Rigidbody>().velocity = Random.onUnitSphere * 5;
			GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 2;
		}
	}
}
