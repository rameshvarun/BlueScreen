using UnityEngine;
using System.Collections;

public class UpDownLook : MonoBehaviour {

	private float angle = 0.0f;
	public Quaternion rotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		angle -= Input.GetAxis("Mouse Y")*5;
		angle = Mathf.Clamp(angle, -60, 60);
		transform.localRotation = rotation * Quaternion.AngleAxis(angle, Vector3.right);
	}
}
