using UnityEngine;
using System.Collections;

public class Disappear : MonoBehaviour {

	public float time;
	private float timer;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > time) {
			Destroy(gameObject);
		}
	}
}
