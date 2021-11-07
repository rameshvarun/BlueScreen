using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class LeverScript : MonoBehaviour {
	bool pulled = false;

	public Transform light;
	public Transform beacon;
	public Material greenMaterial;

	public AudioClip audio;

	private bool clickable;
	private PlayerInput input;

	// Use this for initialization
	void Start () {
		clickable = false;
		input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
	}

	public bool isClickable()
    {
		return clickable;
    }
	
	// Update is called once per frame
	void Update () {
		clickable = false;
		if(!pulled) {
			// Check for entrance into cyberspace
			RaycastHit hit;
			Transform cam = Camera.main.transform;
			if(Physics.Raycast(cam.position, cam.forward, out hit)) {
				if(hit.collider.gameObject == gameObject && Vector3.Distance(cam.position, hit.point) < 3.0f ) {
					clickable = true;
					if(input.actions["Interact"].triggered) {
						pulled = true;
						this.light.GetComponent<Light>().color = Color.green;
						beacon.GetComponent<MeshRenderer>().material = greenMaterial;

						AudioSource.PlayClipAtPoint(audio, transform.position);
					}
				}
			}
		}
		else {
			Transform handle = this.transform.Find("leverhandle");
			handle.localRotation = Quaternion.Lerp(handle.localRotation, Quaternion.Euler(365, -90, 90), Time.deltaTime * 5.0f);
		}
	}

	public bool isPulled() {
		return pulled;
	}
}
