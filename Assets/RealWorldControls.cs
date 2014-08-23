using UnityEngine;
using System.Collections;

public class RealWorldControls : MonoBehaviour {
	private bool grounded = false;
	public float speed;

	public float maxDeltaVel = 10.0f;
	public float jumpHeight = 2.0f;

	public bool canJump;

	private Quaternion rotation = Quaternion.identity;

	private float currentLean = 0;

	public float standHeight = 2.3f;
	private float height = 2.3f;
	public float crouchHeight = 1.2f;

	// Use this for initialization
	void Start () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X")*5, Vector3.up);

		float leanAngle = Mathf.Lerp(currentLean, Input.GetAxis("Lean") * 70, Time.deltaTime * 5.0f);
		
		Quaternion leanRotate = Quaternion.AngleAxis(leanAngle, Vector3.forward);
		rigidbody.rotation = rotation * leanRotate;
		
		if(grounded) {
			Vector3 targetVel = speed*transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

			if(Input.GetButton("Crouch")) {
				height = crouchHeight;
				targetVel *= 0.7f;
			}
			else {
				height = Mathf.Lerp(height, standHeight, Time.deltaTime * 5.0f);
			}
			this.GetComponent<CapsuleCollider>().height = height;

			Vector3 deltaVel = targetVel - rigidbody.velocity;
			deltaVel.x = Mathf.Clamp(deltaVel.x, -maxDeltaVel, maxDeltaVel);
			deltaVel.z = Mathf.Clamp(deltaVel.z, -maxDeltaVel, maxDeltaVel);
			deltaVel.y = 0;
			rigidbody.AddForce(deltaVel, ForceMode.VelocityChange);

			if(canJump && Input.GetButton("Jump")) {
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,
				                                 Mathf.Sqrt(2 * jumpHeight * ( -Physics.gravity.y) ),
				                                 rigidbody.velocity.z);
			}




		}

		rigidbody.AddForce(Physics.gravity * rigidbody.mass);

		grounded = false;
	}

	void OnCollisionStay(Collision collision) {
		foreach(ContactPoint point in collision.contacts) {
			float slope = Vector3.Dot(point.normal, Vector3.up);
			if(slope > 0.8) {
				grounded = true;
			}
		}

	}
}
