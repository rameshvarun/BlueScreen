using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float life = 10.0f;
	private float lifeTimer = 0.0f;

	public float speed = 5.0f;

	public float power = 1.0f;

	public AudioClip spawnSound;
	public AudioClip hitSound;

	public int side = 0;

	// Use this for initialization
	void Start () {
		AudioSource.PlayClipAtPoint(spawnSound, transform.position, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		lifeTimer += Time.deltaTime;
		if(lifeTimer > life) {
			Destroy(this.gameObject);
		}

		Vector3 newPosition = transform.position + transform.forward * Time.deltaTime * speed;

		RaycastHit hit;
		Ray ray = new Ray(transform.position, transform.forward);
		if(Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, newPosition))) {
			bool ishit = (side == 0) ? hit.collider.tag != "Player" : hit.collider.tag != "Enemy";
			if(ishit) {
				Destroy(this.gameObject);
				hit.collider.gameObject.SendMessage("Hit", power);
				AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.1f);
			}
		}
		transform.position = newPosition;
	}
}
