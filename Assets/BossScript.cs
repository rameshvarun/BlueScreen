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
			rigidbody.velocity = Random.onUnitSphere * 5;
			rigidbody.angularVelocity = Random.onUnitSphere * 2;
		}
	}
}
