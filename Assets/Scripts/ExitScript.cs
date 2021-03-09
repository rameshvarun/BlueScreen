using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {

	bool exiting = false;
	float time = 0.0f;

	public int level = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(exiting) {
			time += Time.deltaTime;
			if(time > 2.0) {
				Application.LoadLevel(level);
			}
		}
	
	}

	public void Exit() {
		exiting = true;
	}
}
