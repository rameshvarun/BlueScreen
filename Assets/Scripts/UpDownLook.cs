using UnityEngine;
using System.Collections;

public class UpDownLook : MonoBehaviour {

	public float verticalAngle = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		verticalAngle -= Input.GetAxis("Mouse Y")*5;
		verticalAngle = Mathf.Clamp(verticalAngle, -60, 60);

		transform.localRotation = Quaternion.AngleAxis(verticalAngle, Vector3.right);
	}
}
