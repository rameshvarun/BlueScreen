using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

	public int state = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float newAlpha = GetComponent<Image>().color.a;
		if(state == 1) {
			newAlpha = Mathf.Lerp(GetComponent<Image>().color.a, 1.0f, Time.deltaTime * 5.0f);
		}
		if(state == 0) {
			newAlpha = Mathf.Lerp(GetComponent<Image>().color.a, 0.0f, Time.deltaTime * 5.0f);
		}

		GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, newAlpha);
	}

	public void FadeIn() {
		state = 1;

	}

	public void FadeOut() {
		state = 0;
	}
}
