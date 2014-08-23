using UnityEngine;
using System.Collections;

public class CyberspaceControls : MonoBehaviour {

	public float forceMultiplier;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate () {
		// Movement
		rigidbody.AddRelativeForce(forceMultiplier * Input.GetAxis("Horizontal") * Vector3.right );
		rigidbody.AddRelativeForce(forceMultiplier * Input.GetAxis("Vertical") * Vector3.forward );

		// Rolling
		rigidbody.rotation *= Quaternion.AngleAxis(Input.GetAxis("Lean"), Vector3.forward);


		// Horizontal view rotation
		rigidbody.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X")*5, Vector3.up);
		rigidbody.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y")*5, Vector3.right);
	}
}
