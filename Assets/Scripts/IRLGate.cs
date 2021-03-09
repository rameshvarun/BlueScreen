using UnityEngine;
using System.Collections;

public class IRLGate : MonoBehaviour {

	public Transform lever1;
	public Transform lever2;

	private Vector3 targetPos;
	// Use this for initialization
	void Start () {
		targetPos = transform.position - new Vector3(0, -5, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(lever1.GetComponent<LeverScript>().isPulled() && lever2.GetComponent<LeverScript>().isPulled()) {
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
		}
	}
}
