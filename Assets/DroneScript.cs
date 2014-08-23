using UnityEngine;
using System.Collections;

public class DroneScript : MonoBehaviour {

	public Transform[] path;
	private int currentPath = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(path.Length > 0) {
			Vector3 targetPosition = path[currentPath].position;

			if(Vector3.Distance(transform.position, targetPosition) > 1.0f) {
				Vector3 delta = targetPosition - transform.position;
				rigidbody.AddForce(delta.normalized * 2.0f);
			}

			float dangle = Vector3.Angle(transform.forward, path[currentPath].forward);
			if(dangle > 5.0f) {
				Vector3 cross = Vector3.Cross(transform.forward, path[currentPath].forward);
				rigidbody.AddTorque((cross * dangle).normalized * 2.0f);
			}

			dangle = Vector3.Angle(transform.up, path[currentPath].up);
			if(dangle > 5.0f) {
				Vector3 cross = Vector3.Cross(transform.up, path[currentPath].up);
				rigidbody.AddTorque((cross * dangle).normalized * 2.0f);
			}

		}

		// Slight gravity
		rigidbody.AddForce(Vector3.down * 0.2f);
	}
}
