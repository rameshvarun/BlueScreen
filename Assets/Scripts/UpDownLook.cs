using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class UpDownLook : MonoBehaviour {

	public float verticalAngle = 0.0f;

	private PlayerInput input;

	private OnScreenTrackpad trackpad;

	// Use this for initialization
	void Start () {
		input = GetComponentInParent<PlayerInput>();
		trackpad = GameObject.FindObjectOfType<OnScreenTrackpad>(true);
	}
	
	// Update is called once per frame
	void Update () {
		verticalAngle -= input.actions["Look"].ReadValue<Vector2>().y + trackpad.getDelta().y;
		verticalAngle = Mathf.Clamp(verticalAngle, -60, 60);

		transform.localRotation = Quaternion.AngleAxis(verticalAngle, Vector3.right);
	}
}
