using UnityEngine;
using System.Collections;

public class GateScript : MonoBehaviour {

	public Transform[] enemies;
	private bool active = true;

	public AudioClip gate;
	private float deadTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(active) {
			bool alldead = true;
			foreach(Transform enemy in enemies) {
				if(enemy) alldead = false;
			}
			if(alldead) {
				active = false;
				AudioSource.PlayClipAtPoint(gate, transform.position);
			}
		}
		else {
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 5.0f);
			deadTime += Time.deltaTime;

			if(deadTime > 2.0f) {
				Destroy(gameObject);
			}
		}
	}
}
