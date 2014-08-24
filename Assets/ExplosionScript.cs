using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	public AudioClip explosion;
	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
		AudioSource.PlayClipAtPoint(explosion, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 4.0f);
		timer += Time.deltaTime;

		if(timer > 2.0f) {
			Destroy(gameObject);
		}
	}
}
