using UnityEngine;
using System.Collections;

public class ApartmentEndScript : MonoBehaviour {

	private GameObject player;
	private float timer = 0.0f;

	public int level;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void FixedUpdate () {
		if(player.transform.position.z > transform.position.z) {
			player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10));
			timer += Time.fixedDeltaTime;

			if(timer > 6.0f) {
				GameObject.Find("BlackFader").GetComponent<Fader>().FadeIn();

			}
			if(timer > 8.0f) {
				Application.LoadLevel(level);
			}
		}
	}
}
