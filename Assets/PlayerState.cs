using UnityEngine;
using System.Collections;

public enum GameState {
	Cyberspace,
	RealWorld,
	CyberspaceToRealWorld,
	RealWorldToCyberspace
}

public class PlayerState : MonoBehaviour {

	public static GameState state = GameState.RealWorld;

	public GUITexture clickableTexture;
	public GUITexture whiteFader;

	private CyberspaceEntrance entrance;
	private int transitionPhase = 0;
	private float transitionTime = 0;

	private CyberspaceExit exit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		clickableTexture.enabled = false;
		if(state == GameState.RealWorld) {
			// Disable wireframe
			//GL.wireframe = false;

			// Body has physics
			GetComponent<Rigidbody>().detectCollisions = true;
			GetComponent<Rigidbody>().isKinematic = false;

			// Enable Realworld controls
			GetComponent<RealWorldControls>().enabled = true;
			GetComponent<CapsuleCollider>().enabled = true;
			Camera.main.GetComponent<UpDownLook>().enabled = true;

			// Disable cyberspace controls
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<CyberspaceControls>().enabled = false;

			// Camera is at head
			Camera.main.transform.localPosition = new Vector3(0, 1.827885f, 0);

			// Check for entrance into cyberspace
			RaycastHit hit;
			Transform cam = Camera.main.transform;
			if(Physics.Raycast(cam.position, cam.forward, out hit)) {
				CyberspaceEntrance entrance = hit.collider.gameObject.GetComponent<CyberspaceEntrance>();
				if(entrance && Vector3.Distance(cam.position, hit.point) < 3.0f ) {
					clickableTexture.enabled = true;

					// TODO: Work with controllers
					if(Input.GetMouseButton(0)) {
						state = GameState.RealWorldToCyberspace;
						this.entrance = entrance;
						transitionPhase = 0;
						transitionTime = 0;
					}
				}
			}
		}
		if(state == GameState.Cyberspace) {
			// Object is dynamic
			GetComponent<Rigidbody>().detectCollisions = true;
			GetComponent<Rigidbody>().isKinematic = false;

			// Real-controls are disabled
			GetComponent<CapsuleCollider>().enabled = false;
			GetComponent<RealWorldControls>().enabled = false;
			Camera.main.GetComponent<UpDownLook>().enabled = false;

			// Enable cyberspace controls
			GetComponent<SphereCollider>().enabled = true;
			GetComponent<CyberspaceControls>().enabled = true;

			// Camera is centered on rigid body
			Camera.main.transform.localPosition = new Vector3(0, 0, 0);

			// Check for exit cyberspace
			RaycastHit hit;
			Transform cam = Camera.main.transform;
			if(Physics.Raycast(cam.position, cam.forward, out hit)) {
				CyberspaceExit exit = hit.collider.gameObject.GetComponent<CyberspaceExit>();
				if(exit && Vector3.Distance(cam.position, hit.point) < 0.5f ) {
					clickableTexture.enabled = true;
					
					// TODO: Work with controllers
					if(Input.GetMouseButton(0)) {
						state = GameState.CyberspaceToRealWorld;
						this.exit = exit;
						transitionPhase = 0;
						transitionTime = 0;
					}
				}
			}
		}
		if(state == GameState.RealWorldToCyberspace) {
			// Disable collisions
			GetComponent<Rigidbody>().detectCollisions = false;
			GetComponent<Rigidbody>().isKinematic = true;

			// Disable real world controls
			GetComponent<CapsuleCollider>().enabled = false;
			GetComponent<RealWorldControls>().enabled = false;
			Camera.main.GetComponent<UpDownLook>().enabled = false;
			Camera.main.GetComponent<UpDownLook>().rotation = Quaternion.identity;

			// Disable cyberspace controls
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<CyberspaceControls>().enabled = false;

			// Fly to front of entrance
			if(transitionPhase == 0) {
				Vector3 targetPosition = entrance.Entrance.transform.position + Vector3.down*1.827f;
				Vector3 targetLookat = entrance.FlyTo.transform.position + Vector3.down*1.827f;

				float lerpSpeed = 4.0f;

				Quaternion targetOrientation = Quaternion.LookRotation(targetLookat - targetPosition);
				rigidbody.position = Vector3.Lerp(rigidbody.position, targetPosition, Time.deltaTime * lerpSpeed);
				rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, targetOrientation, Time.deltaTime * lerpSpeed);
			
				Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.identity, Time.deltaTime * lerpSpeed);
				transitionTime += Time.deltaTime;
				if(transitionTime > 1.0f) {
					transitionTime = 0;
					transitionPhase = 1;
				}
			}

			// Wait a bit
			if(transitionPhase == 1) {
				transitionTime += Time.deltaTime;
				if(transitionTime > 0.5f) {
					transitionTime = 0;
					transitionPhase = 2;
				}
			}

			// Fly into entrance
			if(transitionPhase == 2) {
				Vector3 targetPosition = entrance.FlyTo.transform.position + Vector3.down*1.827f;
				float lerpSpeed = 4.0f;

				rigidbody.position = Vector3.Lerp(rigidbody.position, targetPosition, Time.deltaTime * lerpSpeed);

				whiteFader.GetComponent<Fader>().FadeIn();

				transitionTime += Time.deltaTime;
				if(transitionTime > 1.5f) {
					state = GameState.Cyberspace;
					rigidbody.position = entrance.Exit.transform.position;
					whiteFader.GetComponent<Fader>().FadeOut();

					// Enable Wireframe
					Camera.main.GetComponent<Wireframe>().enabled = true;

					// Make Cyberspace visible
					Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("CyberSpace"));
					// Make realworld disappear
					Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("RealWorld"));
				}
			}
		}
		if(state == GameState.CyberspaceToRealWorld) {

			transitionTime += Time.deltaTime;
			if(transitionTime < 1.0f) {
				whiteFader.GetComponent<Fader>().FadeIn();
			}
			if(transitionTime > 2.0f) {
				// Disable Wireframe
				Camera.main.GetComponent<Wireframe>().enabled = false;
				
				// Make Cyberspace disappear
				Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("CyberSpace"));
				// Make realworld visible
				Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("RealWorld"));

				state = GameState.RealWorld;
				rigidbody.position = exit.Exit.transform.position;
				GetComponent<RealWorldControls>().rotation = exit.Exit.transform.rotation;
				whiteFader.GetComponent<Fader>().FadeOut();
			}
		}
	}
}
