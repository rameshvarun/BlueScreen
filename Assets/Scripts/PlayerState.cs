using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum GameState {
	Cyberspace,
	RealWorld,
	CyberspaceToRealWorld,
	RealWorldToCyberspace,
	Death
}

public class PlayerState : MonoBehaviour {

	public static GameState state;

	public Image clickableTexture;
	public Image whiteFader;
	public Image crosshair;
	public Image healthFader;
	public Image blackFader;

	public AudioClip realToCyber;
	public AudioClip cyberToReal;
	public AudioClip hurt;

	private CyberspaceEntrance entrance;
	private int transitionPhase = 0;
	private float transitionTime = 0;

	private CyberspaceExit exit;

	public float maxHealth = 5.0f;
	public float health;
	public float healRate = 0.5f;

	private PlayerInput input;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		state = GameState.RealWorld;

		input = GetComponent<PlayerInput>();
	}

	void Hit(float power) {
		health -= power;
		AudioSource.PlayClipAtPoint(hurt, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		Screen.lockCursor = true;

		// Update health transparency
		float newAlpha = Mathf.Lerp(0, 0.4f, (maxHealth - health)/maxHealth);
		newAlpha = Mathf.Clamp(newAlpha, 0.0f, 0.4f);
		healthFader.color = new Color(healthFader.color.r, healthFader.color.g, healthFader.color.b, newAlpha);

		// Heal rate
		health += healRate * Time.deltaTime;
		if(health > maxHealth)
			health = maxHealth;

		// Death
		if(health < 0) {
			if(state != GameState.Death) {
				state = GameState.Death;
				blackFader.GetComponent<Fader>().FadeIn();
				transitionTime = 0;
			}
		}

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
					if(input.actions["Interact"].triggered) {
						state = GameState.RealWorldToCyberspace;
						this.entrance = entrance;
						transitionPhase = 0;
						transitionTime = 0;

						AudioSource.PlayClipAtPoint(realToCyber, transform.position);
					}
				}
			}

			// Check for levers
			foreach (var lever in GameObject.FindObjectsOfType<LeverScript>())
            {
				if (lever.isClickable())
                {
					clickableTexture.enabled = true;
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
					if(input.actions["Interact"].triggered) {
						state = GameState.CyberspaceToRealWorld;
						this.exit = exit;
						transitionPhase = 0;
						transitionTime = 0;

						AudioSource.PlayClipAtPoint(cyberToReal, transform.position);

						// Switch music
						GameObject.Find("RealMusic").GetComponent<AudioSource>().volume = 0.2f;
						GameObject.Find("CyberMusic").GetComponent<AudioSource>().volume = 0.0f;
					}
				}

				ExitScript levelexit = hit.collider.gameObject.GetComponent<ExitScript>();
				if (levelexit && Vector3.Distance(cam.position, hit.point) < 0.5f)
				{
					clickableTexture.enabled = true;

					if (input.actions["Interact"].triggered && !levelexit.exiting)
					{
						levelexit.Exit();
						whiteFader.GetComponent<Fader>().FadeIn();

						AudioSource.PlayClipAtPoint(cyberToReal, transform.position);
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

			// Reset vertical angle.
			Camera.main.GetComponent<UpDownLook>().verticalAngle = 0;

			// Enable cyberspace controls
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<CyberspaceControls>().enabled = false;

			// Make sure footsteps stopped
			GetComponent<AudioSource>().Stop();
			
			// Fly to front of entrance
			if(transitionPhase == 0) {
				Vector3 targetPosition = entrance.Entrance.transform.position;// + Vector3.down*1.827f;
				Vector3 targetLookat = entrance.FlyTo.transform.position;// + Vector3.down*1.827f;

				float lerpSpeed = 4.0f;

				Quaternion targetOrientation = Quaternion.LookRotation(targetLookat - targetPosition);

				targetPosition -= targetOrientation*Vector3.up*1.827f;

				GetComponent<Rigidbody>().position = Vector3.Lerp(GetComponent<Rigidbody>().position, targetPosition, Time.deltaTime * lerpSpeed);
				GetComponent<Rigidbody>().rotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetOrientation, Time.deltaTime * lerpSpeed);
			
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

				GetComponent<Rigidbody>().position = Vector3.Lerp(GetComponent<Rigidbody>().position, targetPosition, Time.deltaTime * lerpSpeed);

				whiteFader.GetComponent<Fader>().FadeIn();

				transitionTime += Time.deltaTime;
				if(transitionTime > 1.5f) {
					state = GameState.Cyberspace;
					GetComponent<Rigidbody>().position = entrance.Exit.transform.position;
					whiteFader.GetComponent<Fader>().FadeOut();

					// Enable Wireframe
					Wireframe.wireframeEnabled = true;

					// Make Cyberspace visible
					Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("CyberSpace"));
					// Make realworld disappear
					Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("RealWorld"));

					//Enable crosshair
					crosshair.enabled = true;

					// Switch music
					GameObject.Find("RealMusic").GetComponent<AudioSource>().volume = 0.0f;
					GameObject.Find("CyberMusic").GetComponent<AudioSource>().volume = 0.2f;
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
				Wireframe.wireframeEnabled = false;
				
				// Make Cyberspace disappear
				Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("CyberSpace"));
				// Make realworld visible
				Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("RealWorld"));

				//Disable crosshair
				crosshair.enabled = false;

				state = GameState.RealWorld;
				GetComponent<Rigidbody>().position = exit.Exit.transform.position;
				GetComponent<RealWorldControls>().rotation = exit.Exit.transform.rotation;
				whiteFader.GetComponent<Fader>().FadeOut();
			}
		}

		if(state == GameState.Death) {
			transitionTime += Time.deltaTime;
			if(transitionTime > 3.0f) {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
}
