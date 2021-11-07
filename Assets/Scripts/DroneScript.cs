using UnityEngine;
using System.Collections;

public class DroneScript : MonoBehaviour {

	private GameObject player;
	public Transform[] path;
	private int currentPath = 0;
	public bool attackDrone = false;

	private float alertTime = 0.0f;
	private float sightedTime = 0.0f;


	public AudioClip whatwasthat;
	private Vector3 lastKnownPosition = Vector3.zero;

	private int fireState = 0;
	private float fireTime = 0.0f;
	public AudioClip fireCharge;
	public AudioClip droneLaser;

	public Transform laser;

	public float power = 2.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void FireAtPlayer() {
		RaycastHit hit;
		Vector3 searchPosition = transform.Find("Searchlight").transform.position;

		if(Physics.Raycast(searchPosition, GetPlayerPosition() - searchPosition, out hit) ) {
			AudioSource.PlayClipAtPoint(droneLaser, hit.point);
			if(hit.collider.tag == "Player") {
				player.SendMessage("Hit", power);
				Instantiate(laser, GetComponent<Rigidbody>().position, Quaternion.LookRotation(GetPlayerPosition() - GetComponent<Rigidbody>().position) );
			}
		}
	}

    Vector3 GetPlayerPosition()
    {
        return player.GetComponent<CapsuleCollider>().bounds.center;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(path.Length > 0) {
			Vector3 targetPosition = (alertTime > 0.0f) ? lastKnownPosition + new Vector3(0, 5.0f, 0) : path[currentPath].position;

			if(!(alertTime > 0.0f)) {
				if(Vector3.Distance(transform.position, targetPosition) > 1.0f) {
					Vector3 delta = targetPosition - transform.position;
					GetComponent<Rigidbody>().AddForce(delta.normalized * 2.0f);
				}
				else {
					currentPath = (currentPath + 1) % path.Length;
				}
			}
			else {
				if(Vector3.Distance(transform.position, GetPlayerPosition()) > 6.0f) {
					Vector3 delta = targetPosition - transform.position;
					GetComponent<Rigidbody>().AddForce(delta.normalized * 4.0f);
				}
			}

			Vector3 targetDirection = (alertTime > 0.0f) ? (GetComponent<Rigidbody>().position - GetPlayerPosition()) : path[currentPath].forward;

			float dangle = Vector3.Angle(transform.forward, targetDirection);
			if(dangle > 5.0f) {
				Vector3 cross = Vector3.Cross(transform.forward, targetDirection);
				GetComponent<Rigidbody>().AddTorque((cross * dangle).normalized * 2.0f);
			}

			// Orient upwards.
			dangle = Vector3.Angle(transform.up, Vector3.up);
			if(dangle > 5.0f) {
				Vector3 cross = Vector3.Cross(transform.up, Vector3.up);
				GetComponent<Rigidbody>().AddTorque((cross * dangle).normalized * 2.0f);
			}
		}

		// Slight gravity
		GetComponent<Rigidbody>().AddForce(Vector3.down * 0.2f);

		if(attackDrone) {
			// Look for player
			RaycastHit hit;
			Vector3 searchPosition = transform.Find("Searchlight").transform.position;

            // Check if the player can be seen on the current frame.
			bool seen = false;

            // Drones can only see the player if they are in the real world.
            if (player.GetComponent<RealWorldControls>().enabled)
            {
                Vector3 playerPosition = GetPlayerPosition();
                if (Physics.Raycast(searchPosition, playerPosition - searchPosition, out hit))
                {
                    if (hit.collider.tag == "Player")
                    {
                        Vector3 dpos = playerPosition - searchPosition;
                        float angle = Mathf.Abs(Vector3.Angle(-transform.forward, dpos));

                        if (angle < 45.0f && dpos.magnitude < 20.0f)
                        {
                            seen = true;
                            lastKnownPosition = playerPosition;
                        }

                    }
                }
            }

			if(seen && alertTime < 0) {
				if(sightedTime < 0) {
					sightedTime = 0;
					AudioSource.PlayClipAtPoint(whatwasthat, transform.position);
				}
				sightedTime += Time.fixedDeltaTime;

				if(sightedTime > 1.467f) {
					alertTime = 5.0f;
					fireState = 0;
					fireTime = 0.0f;
				}
			}
			else {
				sightedTime -= Time.fixedDeltaTime;
			}

			if(!seen) {
				alertTime -= Time.fixedDeltaTime;
			}

			if(seen && alertTime > 0) {
				alertTime = 5.0f;
			}

			if(alertTime > 0) {
				if(!transform.Find("Alarm").GetComponent<AudioSource>().isPlaying)
					transform.Find("Alarm").GetComponent<AudioSource>().Play ();

				fireTime += Time.fixedDeltaTime;
				if(fireState == 0) {
					if(fireTime > 2.0f) {
						fireState = 1;
						fireTime = 0;
						AudioSource.PlayClipAtPoint(fireCharge, this.transform.position);
					}
				}
				if(fireState == 1) {
					if(fireTime > 2.0f) {
						fireState = 0;
						fireTime = 0;
						FireAtPlayer();
					}
				}
			}
			else {
				if(transform.Find("Alarm").GetComponent<AudioSource>().isPlaying)
					transform.Find("Alarm").GetComponent<AudioSource>().Stop ();
			}
		}
	}

}
