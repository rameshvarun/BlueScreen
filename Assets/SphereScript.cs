using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour {

	private GameObject player;

	public float hp = 5.0f;
	public Transform explosion;

	public float reloadTime = 1.0f;
	private float reloadTimer = 0.0f;
	
	public Transform bullet;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(hp < 0) {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}

		reloadTimer += Time.deltaTime;

		RaycastHit hit;
		if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit)) {
			if(hit.collider.tag == "Player") {
				if(reloadTimer > reloadTime) {
					Instantiate(bullet, transform.position, Quaternion.LookRotation(player.transform.position - transform.position));
					reloadTimer = 0;
				}

			}
		}
	}

	void Hit(float power) {
		hp -= power;
	}
}
