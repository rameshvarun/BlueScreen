using UnityEngine;
using System.Collections;

public class LeverScript : MonoBehaviour {
	bool pulled = false;

	public Transform light;
	public Transform beacon;
	public Material greenMaterial;

	public AudioClip audio;

	public GUITexture clickable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		clickable.enabled = false;
		if(!pulled) {
			// Check for entrance into cyberspace
			RaycastHit hit;
			Transform cam = Camera.main.transform;
			if(Physics.Raycast(cam.position, cam.forward, out hit)) {
				if(hit.collider.gameObject == gameObject && Vector3.Distance(cam.position, hit.point) < 3.0f ) {
					clickable.enabled = true;
					// TODO: Work with controllers
					if(Input.GetMouseButton(0)) {
						pulled = true;
						this.light.light.color = Color.green;
						beacon.GetComponent<MeshRenderer>().material = greenMaterial;

						AudioSource.PlayClipAtPoint(audio, transform.position);
					}
				}
			}
		}
		else {
			Transform handle = this.transform.FindChild("leverhandle");
			handle.localRotation = Quaternion.Lerp(handle.localRotation, Quaternion.Euler(365, -90, 90), Time.deltaTime * 5.0f);
		}
	}

	public bool isPulled() {
		return pulled;
	}
}
