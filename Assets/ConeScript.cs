using UnityEngine;
using System.Collections;

public class ConeScript : MonoBehaviour {

	private int stage = 0;
	private float stageTimer = 0.0f;
	private GameObject player;

	public float speed = 5.0f;
	public float power = 1.0f;

	public float hp = 5.0f;

	public Transform explosion;

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

		stageTimer += Time.deltaTime;

		if(stage == 0) {
			RaycastHit hit;
			if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit)) {
				if(hit.collider.tag == "Player") {
					stage = 1;
					stageTimer = 0;
				}
			}
		}
		if(stage == 1) {
			if(stageTimer < 2.0f) {
				Quaternion target = Quaternion.LookRotation(player.transform.position - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * 5.0f);
			}
			else {
				stage = 2;
				stageTimer = 0;
			}
		}

		if(stage == 2) {
			Vector3 newPosition = transform.position + transform.forward * Time.deltaTime * speed;

			RaycastHit hit;
			Ray ray = new Ray(transform.position, transform.forward);
			if(Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, newPosition))) {
				if(hit.collider.tag != "Enemy") {
					if(hit.collider.tag == "Player") {
						hit.collider.gameObject.SendMessage("Hit", power);
					}

					stage = 3;
					stageTimer = 0;
				}
			}

			transform.position = newPosition;
		}
		if(stage == 3) {
			if(stageTimer > 2.0f) {
				stage = 0;
				stageTimer = 0;
			}
		}
	}

	void Hit(float power) {
		hp -= power;
	}
}
